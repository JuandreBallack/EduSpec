using EduSpec.Controllers;
using EduSpec.Models;
using System.Collections;
using System.Linq;
using System.Web.UI;

namespace EduSpec
{


    public abstract class ItemsData : IHierarchicalEnumerable, IEnumerable
    {
        public abstract IEnumerable Data { get; }

        public IEnumerator GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return (IHierarchyData)enumeratedItem;
        }
    }

    public class ItemData : IHierarchyData
    {
        public string Text { get; protected set; }
        public string NavigateUrl { get; protected set; }
        public string Name { get; protected set; }

        public ItemData(string text, string navigateUrl, string name)
        {
            Text = text;
            NavigateUrl = navigateUrl;
            Name = name;
        }

        public ItemData(string text)
        {
            Text = text;
        }

        bool IHierarchyData.HasChildren
        {
            get
            {
                return HasChildren();
            }
        }
        object IHierarchyData.Item
        {
            get
            {
                return this;
            }
        }
        string IHierarchyData.Path
        {
            get
            {
                return NavigateUrl;
            }
        }
        string IHierarchyData.Type
        {
            get
            {
                return GetType().ToString();
            }
        }
        IHierarchicalEnumerable IHierarchyData.GetChildren()
        {
            return CreateChildren();
        }
        IHierarchyData IHierarchyData.GetParent()
        {
            return null;
        }

        protected virtual bool HasChildren()
        {
            return false;
        }
        protected virtual IHierarchicalEnumerable CreateChildren()
        {
            return null;
        }

        public class CategoriesData : ItemsData
        {
            public override IEnumerable Data
            {
                get
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        if(UserUtils.CurrentUser().ImpersonationUserID != -1)
                            return from category in Context.NavBarCategoriesData(UserUtils.CurrentUser().ImpersonationUserID).ToList() select new CategoryData(category);
                        else
                            return from category in Context.NavBarCategoriesData(UserUtils.CurrentUser().UserID).ToList() select new CategoryData(category);
                    }
                }
            }
        }

        public class CategoryData : ItemData
        {
            public NavBarCategoriesDataResult Category { get; protected set; }

            public CategoryData(NavBarCategoriesDataResult category)
                : base(category.CategoryName)
            {
                Category = category;
            }

            protected override bool HasChildren()
            {
                return true;
            }
            protected override IHierarchicalEnumerable CreateChildren()
            {
                return new NavBarNodesData(Category.CategoryID);
            }
        }

        public class NavBarNodesData : ItemsData
        {
            public int CategoryID { get; protected set; }

            public NavBarNodesData(int categoryID)
                : base()
            {
                CategoryID = categoryID;
            }

            public override IEnumerable Data
            {
                get
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        if (UserUtils.CurrentUser().ImpersonationUserID != -1)
                            return from NavBarNode in Context.NavBarNodesData(CategoryID, UserUtils.CurrentUser().ImpersonationUserID).ToList() select new NavBarNodeData(NavBarNode);
                        else
                            return from NavBarNode in Context.NavBarNodesData(CategoryID, UserUtils.CurrentUser().UserID).ToList() select new NavBarNodeData(NavBarNode);

                        //return from NavBarNode in Context.NavBarNodesData(CategoryID, WebSecurity.CurrentUserId).ToList()
                        //        select new NavBarNodeData(NavBarNode);
                    }
                }
            }
        }

        public class NavBarNodeData : ItemData
        {
            public NavBarNodeData(NavBarNodesDataResult NavBarNode)
                : base(NavBarNode.NavBarNodesName, NavBarNode.URL, NavBarNode.Name)
            {
            }
        }
    }
}
