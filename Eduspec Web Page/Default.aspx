<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%--<div class="jumbotron">
      <div class="container">
        <h1>Devexpress ASP.NET Bootstrap</h1>
        <p>The power and simplicity of server-side Devexpress WebForms Controls, combined with the client-side responsiveness and render code clarity of Bootstrap framework, bringing you the best of two worlds. </p>
        <p><dx:BootstrapButton ID="Button7" runat="server" Text="Learn more &raquo;" AutoPostBack="false" CssClasses-Button="btn-primary btn-lg">
            </dx:BootstrapButton></p>
      </div>
    </div>--%>

    <div id="myCarousel" class="carousel slide" data-ride="carousel">
        <ol class="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
            <li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>
            <%--<li data-target="#myCarousel" data-slide-to="3"></li>--%>
        </ol>

      <!-- Wrapper for slides -->
      <div class="carousel-inner">
          <div class="item active">
              <img src="Content/Images/Slide1.png" alt="Los Angeles" style="width:100%"/>
          </div>
          
          <%--<div class="item">
              <img src="Content/Images/Slide2.png" alt="Chicago" style="width:100%"/>
          </div>--%>
          
          <div class="item">
              <img src="Content/Images/Slide3.png" alt="New York" style="width:100%"/>
          </div>

          <div class="item">
              <img src="Content/Images/Slide4.png" alt="New York" style="width:100%"/>
          </div>
      </div>

      <!-- Left and right controls -->
      <a class="left carousel-control" href="#myCarousel" data-slide="prev">
        <span class="glyphicon glyphicon-chevron-left"></span>
        <span class="sr-only">Previous</span>
      </a>
      <a class="right carousel-control" href="#myCarousel" data-slide="next">
        <span class="glyphicon glyphicon-chevron-right"></span>
        <span class="sr-only">Next</span>
      </a>
    </div>

    <div class="container">
      <!-- Example row of columns -->
      <div class="row">
        <div class="col-md-4">
          <h2>Efficiency</h2>
          <p>In the current economic climate in South Africa the efficiency of the office staff is critical. This is due to the shortage in staff and therefore staff is overloaded with their daily tasks. The implementation of efficient systems is from utmost importance.</p>
          <%--<p><dx:BootstrapButton ID="Button1" runat="server" Text="View details &raquo;" AutoPostBack="false">
            </dx:BootstrapButton></p>--%>
        </div>
        <div class="col-md-4">
          <h2>Control</h2>
          <p>Our easy to use web application give you control over the application for exemption process. An easy to follow structured process flow in the application keep track of all the interactions on the application.</p>
          <%--<p><dx:BootstrapButton ID="Button2" runat="server" Text="View details &raquo;" AutoPostBack="false">
            </dx:BootstrapButton></p>--%>
       </div>
        <div class="col-md-4">
          <h2>Convenience</h2>
          <p>EduSpec being a cloud base application make it convenient to access from anywhere. Having the ability to add as many users you require to the system makes it easy and convenient for any user to view the process and status of any application.</p>
          <%--<p><dx:BootstrapButton ID="Button3" runat="server" Text="View details &raquo;" AutoPostBack="false">
            </dx:BootstrapButton></p>--%>
        </div>
      </div>
<%--      <div class="row">
        <div class="col-md-4">
          <h2>Visually Consistent</h2>
          <p>Focus on business and stop wasting cycles on design. Leave it to Bootstrap's myriad of available visual themes to guarantee consistent look and feel throughout your web application - from early prototype to production. </p>
          <p><dx:BootstrapButton ID="Button4" runat="server" Text="View details &raquo;" AutoPostBack="false">
            </dx:BootstrapButton></p>
        </div>
        <div class="col-md-4">
          <h2>An Extensive Control Suite</h2>
          <p>The DevExpress Bootstrap Control Suite includes a GridView with powerful data analysis and display features, Data Editors with automatic layout management, Navigation Controls, and fast and lightweight Chart controls. </p>
          <p><dx:BootstrapButton ID="Button5" runat="server" Text="View details &raquo;" AutoPostBack="false">
            </dx:BootstrapButton></p>
       </div>
        <div class="col-md-4">
          <h2>Powered by ASP.NET Controls</h2>
          <p>New render comes with familiar feature set. Expect the same level of server-side and client-side API as in DevExpress ASP.NET WebForms Controls. And yes, we made sure that transition to the new suite is a breeze. </p>
          <p><dx:BootstrapButton ID="Button6" runat="server" Text="View details &raquo;" AutoPostBack="false">
            </dx:BootstrapButton></p>
        </div>
      </div>--%>
     </div>
</asp:Content>