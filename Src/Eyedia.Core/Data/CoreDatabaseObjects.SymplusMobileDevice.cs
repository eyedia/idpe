#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Deb'jyoti Das
Created - 3/19/2013 11:14:16 AM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Eyedia.Core.Data;
using Eyedia.Core;
using System.Data;
using Eyedia.Core.MobileDevices;

namespace Eyedia.Core.Data
{
    public partial class CoreDatabaseObjects
    {
        public int Save(SymplusMobileDevice device, string clientIpAddress)
        {
            if (string.IsNullOrEmpty(device.DeviceId))
                throw new Exception("Device id can not be null or empty!");

            if (string.IsNullOrEmpty(device.RegistrationId))
                throw new Exception("Registration id can not be null or empty!");

            SymplusMobileDevice existingDevice = GetDevice(device.DeviceId);
            int returnId = 0;
            string sqlStatement = string.Empty;         

            if (existingDevice == null)
            {
                sqlStatement = "insert into [SymplusMobileDevice] ([DeviceId], [RegistrationId], [OperatingSystem], [CreatedOn], [Source]) ";
                sqlStatement += string.Format("values( '{0}','{1}',{2},'{3}','{4}')"
                    , device.DeviceId, device.RegistrationId, device.OperatingSystem, DateTime.Now, clientIpAddress);
                CoreDatabaseObjects.Instance.ExecuteStatement(sqlStatement);
                returnId = GetMaxId("SymplusMobileDevice");
              
            }
            else
            {
                sqlStatement = string.Format("update [SymplusMobileDevice] set [RegistrationId] = '{0}', [UpdatedOn] = '{1}', [Source] = '{2}' where  [DeviceId] = '{3}'",
                    device.RegistrationId, DateTime.Now, clientIpAddress, device.DeviceId);
                CoreDatabaseObjects.Instance.ExecuteStatement(sqlStatement);
                returnId = existingDevice.Id;
            }


            return returnId;
        }

        public List<SymplusMobileDevice> GetDevices()
        {

            string commandText = "select [Id],[DeviceId],[RegistrationId],[OperatingSystem],[CreatedOn],[UpdatedOn],[Source] from [SymplusMobileDevice]";               

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return null;
            if (table.Rows.Count == 0)
                return null;

            List<SymplusMobileDevice> devices = new List<SymplusMobileDevice>();

            foreach (DataRow row in table.Rows)
            {
                SymplusMobileDevice device = new SymplusMobileDevice();

                device.Id = (int)row["Id"].ToString().ParseInt();
                device.DeviceId = row["DeviceId"].ToString();
                device.RegistrationId = row["RegistrationId"].ToString();
                device.OperatingSystem = (int)row["OperatingSystem"].ToString().ParseInt();
                device.CreatedOn = (DateTime)(row["CreatedOn"] != DBNull.Value ? row["CreatedOn"].ToString().ParseDateTime() : DateTime.MinValue);
                device.UpdatedOn = (DateTime)(row["UpdatedOn"] != DBNull.Value ? row["UpdatedOn"].ToString().ParseDateTime() : DateTime.MinValue);
                device.Source = row["Source"].ToString();
                devices.Add(device);
            }
            return devices;
        }

        public List<SymplusMobileDevice> GetDevices(MobileDeviceOperatingSystems operatingSystem)
        {

            string commandText = "select [Id],[DeviceId],[RegistrationId],[OperatingSystem],[CreatedOn],[UpdatedOn],[Source] from [SymplusMobileDevice] where [OperatingSystem] = " + (int)operatingSystem;

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return null;
            if (table.Rows.Count == 0)
                return null;

            List<SymplusMobileDevice> devices = new List<SymplusMobileDevice>();

            foreach (DataRow row in table.Rows)
            {
                SymplusMobileDevice device = new SymplusMobileDevice();

                device.Id = (int)row["Id"].ToString().ParseInt();
                device.DeviceId = row["DeviceId"].ToString();
                device.RegistrationId = row["RegistrationId"].ToString();
                device.OperatingSystem = (int)row["OperatingSystem"].ToString().ParseInt();
                device.CreatedOn = (DateTime)(row["CreatedOn"] != DBNull.Value ? row["CreatedOn"].ToString().ParseDateTime() : DateTime.MinValue);
                device.UpdatedOn = (DateTime)(row["UpdatedOn"] != DBNull.Value ? row["UpdatedOn"].ToString().ParseDateTime() : DateTime.MinValue);
                device.Source = row["Source"].ToString();
                devices.Add(device);
            }
            return devices;
        }


        public SymplusMobileDevice GetDevice(string deviceId)
        {

            string commandText = string.Format("select [Id],[DeviceId],[RegistrationId],[OperatingSystem],[CreatedOn],[UpdatedOn],[Source] from [SymplusMobileDevice] where [DeviceId] = '{0}'"
                , deviceId);

            DataTable table = CoreDatabaseObjects.Instance.ExecuteCommand(commandText);
            if (table == null)
                return null;
            if (table.Rows.Count == 0)
                return null;

            SymplusMobileDevice device = new SymplusMobileDevice();

            device.Id = (int)table.Rows[0]["Id"].ToString().ParseInt();
            device.DeviceId = table.Rows[0]["DeviceId"].ToString();
            device.RegistrationId = table.Rows[0]["RegistrationId"].ToString();
            device.OperatingSystem = (int)table.Rows[0]["OperatingSystem"].ToString().ParseInt();
            device.CreatedOn = (DateTime)(table.Rows[0]["CreatedOn"] != DBNull.Value ? table.Rows[0]["CreatedOn"].ToString().ParseDateTime() : DateTime.MinValue);
            device.UpdatedOn = (DateTime)(table.Rows[0]["UpdatedOn"] != DBNull.Value ? table.Rows[0]["UpdatedOn"].ToString().ParseDateTime() : DateTime.MinValue);
            device.Source = table.Rows[0]["Source"].ToString();
            return device;
        }
        

    }
}


