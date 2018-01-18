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

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "IDPE Exported files (*.idpex)|*.idpex|All files (*.*)|*.*";
            saveFileDialog1.FileName = dataSource.Name + ".idpex";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(saveFileDialog1.FileName))
                {
                    if (MessageBox.Show("The file already exists. Do you want to overwrite the file?", "File Already Exists",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }
                    else
                    {
                        return;
                    }
                }
                try
                {
                    DataSourceBundle dsb = new DataSourceBundle();
                    dsb.Export(dataSource.Id, saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

                try
                {
                    dsb.Import();
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
