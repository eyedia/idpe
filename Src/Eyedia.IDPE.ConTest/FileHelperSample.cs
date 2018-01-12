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
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using Symplus.Core.Data;
using Symplus.RuleEngine.DataManager;
using FileHelpers;
using FileHelpers.MasterDetail;
using Symplus.Core;

namespace ConTest
{
    class FileHelperSample
    {
        public static void Execute(string[] args)
        {

            MultiRecordEngine engine
                = new MultiRecordEngine(new RecordTypeSelector(RecordSelector),
                    typeof(FileHeader), typeof(FileTrailer), typeof(BatchHeader), typeof(BatchTrailer), typeof(PaymentRecord), typeof(IgnoreRecord));
            object[] res = engine.ReadFile(args[0]);
            int count = 0;
            DataTable dtPaymentRecords = null;
            foreach (object obj in res)
            {
                if (obj is PaymentRecord)
                {
                    DataTable dt = obj.ToDataTable();
                    if (dtPaymentRecords == null)
                        dtPaymentRecords = dt;
                    else
                        dtPaymentRecords.Rows.Add(dt.Rows[0].ItemArray);
                    count++;
                }
            }
        }

        static Type RecordSelector(MultiRecordEngine engine, string record)
        {
            string[] fields = record.Split("|".ToCharArray());
            if (fields[0] == "FH")
                return typeof(FileHeader);
            else if (fields[0] == "FT")
                return typeof(FileTrailer);
            else if (fields[0] == "BH")
                return typeof(BatchHeader);
            else if (fields[0] == "BT")
                return typeof(BatchTrailer);
            else if (fields[0] == "PR")
                return typeof(PaymentRecord);
            else
                return typeof(IgnoreRecord);
        }

    }

    [DelimitedRecord("|")]
    public class FileHeader
    {
        public string RecordType { get; set; }
        public string FileNumber { get; set; }
        public string FileDate { get; set; }
        public string FileTime { get; set; }

    }

    [DelimitedRecord("|")]
    public class FileTrailer
    {
        public string RecordType { get; set; }
        public string PaymentCount { get; set; }
        public string FilePaymentAmount { get; set; }

    }

    [DelimitedRecord("|")]
    public class BatchHeader
    {
        public string RecordType { get; set; }
        public string BatchNumber { get; set; }
        public string BatchDate { get; set; }
        public string BatchTime { get; set; }

    }

    [DelimitedRecord("|")]
    public class BatchTrailer
    {
        public string RecordType { get; set; }
        public string PaymentCount { get; set; }
        public string BatchPaymentAmount { get; set; }
        public string BatchNumber { get; set; }

    }

    [DelimitedRecord("|")]
    public class PaymentRecord
    {
        public string RecordType { get; set; }
        public string PaymentType { get; set; }
        public string CreditOrDebit { get; set; }
        public string Amount { get; set; }
        public string DestinationRTN { get; set; }
        public string DestinationAccountNumber { get; set; }
        public string OriginatorRTN { get; set; }
        public string OriginatorAccountNumber { get; set; }
        public string EffectiveDate { get; set; }
        public string OriginatorToBeneficiaryinfo1 { get; set; }
        public string OriginatorToBeneficiaryinfo2 { get; set; }
        public string OriginatorToBeneficiaryinfo3 { get; set; }
        public string OriginatorToBeneficiaryinfo4 { get; set; }
        public string BeneficiaryAdviceInfo1 { get; set; }
        public string BeneficiaryAdviceInfo2 { get; set; }
        public string BeneficiaryAdviceInfo3 { get; set; }
        public string SendersReferenceNumber { get; set; }
    }

    [DelimitedRecord("|")]
    public class IgnoreRecord
    {
        [FieldOptional]
        public string Field1;
        [FieldOptional]
        public string Field2;
        [FieldOptional]
        public string Field3;
        [FieldOptional]
        public string Field4;
        [FieldOptional]
        public string Field5;
        [FieldOptional]
        public string Field6;
        [FieldOptional]
        public string Field7;
        [FieldOptional]
        public string Field8;
        [FieldOptional]
        public string Field9;
        [FieldOptional]
        public string Field10;
        [FieldOptional]
        public string Field11;
        [FieldOptional]
        public string Field12;
        [FieldOptional]
        public string Field13;
        [FieldOptional]
        public string Field14;
        [FieldOptional]
        public string Field15;
        [FieldOptional]
        public string Field16;
        [FieldOptional]
        public string Field17;
        [FieldOptional]
        public string Field18;
        [FieldOptional]
        public string Field19;
        [FieldOptional]
        public string Field20;
        [FieldOptional]
        public string Field21;
        [FieldOptional]
        public string Field22;
        [FieldOptional]
        public string Field23;
        [FieldOptional]
        public string Field24;
        [FieldOptional]
        public string Field25;
        [FieldOptional]
        public string Field26;
        [FieldOptional]
        public string Field27;
        [FieldOptional]
        public string Field28;
        [FieldOptional]
        public string Field29;
        [FieldOptional]
        public string Field30;
        [FieldOptional]
        public string Field31;
        [FieldOptional]
        public string Field32;
        [FieldOptional]
        public string Field33;
        [FieldOptional]
        public string Field34;
        [FieldOptional]
        public string Field35;
        [FieldOptional]
        public string Field36;
        [FieldOptional]
        public string Field37;
        [FieldOptional]
        public string Field38;
        [FieldOptional]
        public string Field39;
        [FieldOptional]
        public string Field40;
        [FieldOptional]
        public string Field41;
        [FieldOptional]
        public string Field42;
        [FieldOptional]
        public string Field43;
        [FieldOptional]
        public string Field44;
        [FieldOptional]
        public string Field45;
        [FieldOptional]
        public string Field46;
        [FieldOptional]
        public string Field47;
        [FieldOptional]
        public string Field48;
        [FieldOptional]
        public string Field49;
        [FieldOptional]
        public string Field50;
        [FieldOptional]
        public string Field51;
        [FieldOptional]
        public string Field52;
        [FieldOptional]
        public string Field53;
        [FieldOptional]
        public string Field54;
        [FieldOptional]
        public string Field55;
        [FieldOptional]
        public string Field56;
        [FieldOptional]
        public string Field57;
        [FieldOptional]
        public string Field58;
        [FieldOptional]
        public string Field59;
        [FieldOptional]
        public string Field60;
        [FieldOptional]
        public string Field61;
        [FieldOptional]
        public string Field62;
        [FieldOptional]
        public string Field63;
        [FieldOptional]
        public string Field64;
        [FieldOptional]
        public string Field65;
        [FieldOptional]
        public string Field66;
        [FieldOptional]
        public string Field67;
        [FieldOptional]
        public string Field68;
        [FieldOptional]
        public string Field69;
        [FieldOptional]
        public string Field70;
        [FieldOptional]
        public string Field71;
        [FieldOptional]
        public string Field72;
        [FieldOptional]
        public string Field73;
        [FieldOptional]
        public string Field74;
        [FieldOptional]
        public string Field75;
        [FieldOptional]
        public string Field76;
        [FieldOptional]
        public string Field77;
        [FieldOptional]
        public string Field78;
        [FieldOptional]
        public string Field79;
        [FieldOptional]
        public string Field80;
        [FieldOptional]
        public string Field81;
        [FieldOptional]
        public string Field82;
        [FieldOptional]
        public string Field83;
        [FieldOptional]
        public string Field84;
        [FieldOptional]
        public string Field85;
        [FieldOptional]
        public string Field86;
        [FieldOptional]
        public string Field87;
        [FieldOptional]
        public string Field88;
        [FieldOptional]
        public string Field89;
        [FieldOptional]
        public string Field90;
        [FieldOptional]
        public string Field91;
        [FieldOptional]
        public string Field92;
        [FieldOptional]
        public string Field93;
        [FieldOptional]
        public string Field94;
        [FieldOptional]
        public string Field95;
        [FieldOptional]
        public string Field96;
        [FieldOptional]
        public string Field97;
        [FieldOptional]
        public string Field98;
        [FieldOptional]
        public string Field99;
        [FieldOptional]
        public string Field100;
    }

}


