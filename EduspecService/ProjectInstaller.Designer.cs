namespace EduspecService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EduSpecServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.EduSpecServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // EduSpecServiceProcessInstaller
            // 
            this.EduSpecServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.EduSpecServiceProcessInstaller.Password = null;
            this.EduSpecServiceProcessInstaller.Username = null;
            // 
            // EduSpecServiceInstaller
            // 
            this.EduSpecServiceInstaller.Description = "This is the corresp[ondince server for EduSpec.";
            this.EduSpecServiceInstaller.DisplayName = "EduSpec Service";
            this.EduSpecServiceInstaller.ServiceName = "Service1";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.EduSpecServiceProcessInstaller,
            this.EduSpecServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller EduSpecServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller EduSpecServiceInstaller;
    }
}