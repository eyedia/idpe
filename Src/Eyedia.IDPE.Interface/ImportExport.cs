using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Eyedia.Core;
using Eyedia.IDPE.DataManager;
using Eyedia.IDPE.Common;
using Eyedia.IDPE.Services;
using System.Windows.Forms;

namespace Eyedia.IDPE.Interface
{
    class ImportExport
    {
        public static void Export(int dataSourceId)
        {
            IdpeDataSource dataSource = new Manager().GetDataSourceDetails((int)dataSourceId);
            IdpeDataSource systemDataSource = new Manager().GetDataSourceDetails((int)dataSource.SystemDataSourceId);

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {

                string fileName = Path.Combine(folderBrowser.SelectedPath, dataSource.Name + ".idpex");
                string msg = fileName + Environment.NewLine;
                ExportDataSource(dataSource, fileName);


                fileName = Path.Combine(folderBrowser.SelectedPath, systemDataSource.Name + ".idpex");
                msg += fileName + Environment.NewLine;
                ExportDataSource(systemDataSource, fileName);

                msg += "Saved successfully!";
                MessageBox.Show(msg, "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private static void ExportDataSource(IdpeDataSource dataSource, string fileName)
        {
            if (File.Exists(fileName))
            {
                if (MessageBox.Show("The file " + fileName + " already exists. Do you want to overwrite the file?", "File Already Exists",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    File.Delete(fileName);
                }
                else
                {
                    return;
                }
            }

            try
            {
                DataSourceBundle dsb = new DataSourceBundle();
                dsb.Export(dataSource.Id, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool Import()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "IDPE Exported files (*.idpex)|*.idpex|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataSourceBundle dsb = new DataSourceBundle(openFileDialog1.FileName);
                Manager manager = new Manager();
                if (manager.ApplicationExists(dsb.DataSource.Name))
                {
                    if (MessageBox.Show("A data source with name '" + dsb.DataSource.Name + "' already exist! Do you want to overwrite?", "Import Data Source"
                        , MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                        manager.DeleteDataSource(dsb.DataSource.Name);
                    else
                        return false;
                }

                if ((dsb.DataSource.IsSystem == false)
                    && (manager.ApplicationExists(dsb.SystemDataSourceName) == false))
                {
                    string msg = string.Format("The system data source '{0}' of this data source does not exist. Please import system data source first and then try to import this one!",
                        dsb.SystemDataSourceName);
                    MessageBox.Show(msg, "System Data Source Does Not Exist"
                        , MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                try
                {
                    dsb.Import();
                    string msg = openFileDialog1.FileName + " imported successfully!";
                    if (dsb.DataSource.IsSystem == true)
                        msg += " Validate in Tools-->System Data Sources";
                    
                    MessageBox.Show(msg, 
                        "Import Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return false;
        }     
    }
}
