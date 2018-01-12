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
using System.ComponentModel;

namespace Eyedia.IDPE.Common
{

    public enum AttributeTypes
    {
        [Description("Unknown")]
        Unknown,

        [Description("Int")]
        Int,

        [Description("BigInt")]
        BigInt,

        [Description("Decimal")]
        Decimal,

        [Description("Bit")]
        Bit,

        [Description("Datetime")]
        Datetime,

        [Description("String")]
        String,

        [Description("List of Values")]
        Codeset,

        [Description("Referenced")]
        Referenced,

        [Description("Not Referenced")]
        NotReferenced,

        [Description("Generated")]
        Generated
    }

    public enum AttributePrintValueTypes
    {
        [Description("Value")]
        Value,

        [Description("Print enum code of the LOV value")]
        CodeSetValueEnumCode,

        [Description("Print reference key of the LOV value")]
        CodeSetValueEnumReferenceKey,

        [Description("Print date only")]
        DateTimePrintDate,

        [Description("Print minimum value")]
        DateTimePrintMinValue,

        [Description("Print empty string in case of 0(zero)")]
        NumericPrintNullInCaseOfZero,

        [Description("Print NULL in case Original Value is NULL")]
        PrintNullInCaseOfNull,

        [Description("Print 0(zero) or 1(one)")]
        BitPrintZeroOrOne,

        [Description("Print TRUE or FALSE")]
        BitPrintTrueOrFalse,

        [Description("Print empty string in case of NaN")]
        NumericPrintNullInCaseOfNaN,

        [Description("Print empty string in case of Infinity")]
        NumericPrintNullInCaseOfInfinity,

        [Description("Print empty string in case of NaN or Infinity")]
        NumericPrintNullInCaseOfNaNOrInfinity,

        [Description("Custom Format")]
        Custom = 99
    }

    public enum RuleSetTypes
    {
        /// <summary>
        /// Unknown rule set type, generally prior to initialize
        /// </summary>
        Unknown,
        /// <summary>
        /// Once the data set is received, prevalidate rules are applied, generally to the whole data container, duplicate check is the common example
        /// </summary>
        PreValidate,

        /// <summary>
        /// Before preparing a row, setting default values can be a good example
        /// </summary>
        RowPreparing,

        /// <summary>
        /// After parsing the datatypes, the best place to calculate derrived values
        /// </summary>
        RowPrepared,

        /// <summary>
        /// A row can be marked as valid or invalid. Cell values cannot be changed at this stage
        /// </summary>
        RowValidate,

        /// <summary>
        /// Post valid rules are applied to complete data set
        /// </summary>
        PostValidate,

        /// <summary>
        /// Before pulling Sql query to pull data, these rules will be applied. Best place to set connection string
        /// </summary>
        SqlPullInit
    }

    public enum SqlCommandTypes
    {
        Unknown,
        SqlCommand,
        StoreProcedure
    }


    public enum SreKeyTypes
    {
        Unknown,

        /// <summary>
        /// [String]
        /// Stores global commands raw string, for more information read GlobalEventsOnComplete
        /// </summary>
        GlobalEventsOnComplete,

        /// <summary>
        /// [String]
        /// SQL server connection string
        /// </summary>
        ConnectionStringSqlServer,

        /// <summary>
        /// [String]
        /// Oracle connection string
        /// </summary>
        ConnectionStringOracle,

        /// <summary>
        /// [String]
        /// SQL CE connection string
        /// </summary>
        ConnectionStringSqlCe,

        /// <summary>
        /// [String]
        /// DB2 connection string
        /// </summary>
        ConnectionStringDB2iSeries,

        /// <summary>
        /// [String]
        /// FTP url
        /// </summary>
        FtpRemoteLocation,

        /// <summary>
        /// [String]
        /// FTP user name
        /// </summary>
        FtpUserName,

        /// <summary>
        /// [String]
        /// FTP password
        /// </summary>
        FtpPassword,

        /// <summary>
        /// [Int32]
        /// FTP port
        /// </summary>
        FtpPort,

        /// <summary>
        /// [String]
        /// FTP Watch interval in minutes
        /// </summary>
        FtpWatchInterval,


        /// <summary>
        /// [String]
        /// Local archive folder. SRE sets this internally
        /// </summary>
        LocalLocation,

        /// <summary>
        /// [String]
        /// Watch filter on local file system (e.g - .txt|.csv)
        /// </summary>
        WatchFilter,

        /// <summary>
        /// [String]
        /// Local archive folder. SRE sets this internally
        /// </summary>
        OutputFolder,

        /// <summary>
        /// [String]
        /// Local archive folder. SRE sets this internally
        /// </summary>
        LocalArchiveFolder,

        /// <summary>
        /// [String]
        /// &apos;Fixed Length&apos; schema
        /// </summary>
        FixedLengthSchema,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make last line as header in case of map &apos;Fixed Length&apos;, and will map it to the value.
        /// </summary>
        FixedLengthHeaderAttribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make last line as footer in case of map &apos;Fixed Length&apos;, and will map it to the value.
        /// </summary>
        FixedLengthFooterAttribute,

        /// <summary>
        /// [String]
        /// Sql query, mostly used when loading mechanism is &apos;PullSQL&apos;. If if it is not in use @data source, you can use this for any other purpose.
        /// </summary>
        SqlQuery,

        /// <summary>
        /// [String]
        /// Any custom key. If app owner wants to keep some configuration/information in SRE (later to be used in some plug-ins/interfaces).
        /// </summary>
        Custom,

        /// <summary>
        /// [Bool]
        /// SRE has a mechanism to generate multiple parameters from database. For more information please read &apos;TODO&apos;. This flag enables/disables this feature.
        /// </summary>
        GenerateParametersFromDatabase,

        /// <summary>
        /// [Bool]
        /// If the first row is header. If this is set in case of zip/rar/tar file, this will get applied to all files in the zip
        /// </summary>
        IsFirstRowHeader,

        /// <summary>
        /// [String - 1 char]
        /// When loading mechanism is &apos;PullSQL&apos;, if your SQL query output is directly fed into SRE, this should set as 'D' or if you would like an interface to reprocess the SQL
        /// output and then feed into SRE, you will set this as &apos;I&apos; and also set &apos;PullSqlInterfaceName&apos;
        /// </summary>
        PullSqlReturnType,

        /// <summary>
        /// [String]
        /// When loading mechanism is &apos;PullSQL&apos;, and you would need an interface to process the output of the SQL and feed SRE, which means
        /// you set &apos;PullSqlReturnType = I&apos;
        /// </summary>
        PullSqlInterfaceName,

        /// <summary>
        /// [String]
        /// When loading mechanism is &apos;PullSQL&apos;, you define your connection string using this
        /// </summary>
        PullSqlConnectionString,

        /// <summary>
        /// [Int32]
        /// SQL Watch interval in minutes
        /// </summary>
        SqlWatchInterval,

        /// <summary>
        /// [String]
        ///  The full file interface type, which will be used to process individual files (applicable when &apos;ZipFileConfigType = 1&apos;. This is useful when you have custom data inside
        ///  the file (for example, xml) and you would need an interface to covert the xml into data table.
        /// </summary>
        FileInterfaceName,

        /// <summary>
        /// [String]
        /// The full interface type, which will be used to process the zip (applicable when &apos;ZipFileConfigType = 1&apos;
        /// </summary>
        ZipInterfaceName,

        /// <summary>
        /// [DataFormatTypes]
        /// Zip data file format
        /// </summary>
        ZipDataFileDataFormatType,

        /// <summary>
        /// [string]
        /// -1 = SRE will not sort the files and process as is
        /// 0 = SRE will sort the files based on extensions before processing. Ascending
        /// 1 = SRE will sort the files based on extensions before processing. Descending
        /// ".txt|.dat" = SRE will sort the files based on configured extensions
        /// </summary>
        ZipFilesSortType,

        /// <summary>
        /// [String]
        /// Sql update query, used when loading mechanism is &apos;PullSql&apos;. This query is used to mark data as processed.
        /// </summary>
        SqlUpdateQueryProcessed,

        /// <summary>
        /// [Int]
        /// Spread sheet number of a spreadsheet file
        /// </summary>
        SpreadSheetNumber,

        /// <summary>
        /// [String]
        /// SRE will ignore these types of extensions (e.g - .txt|.dat)
        /// </summary>
        ZipIgnoreFileList,

        /// <summary>
        /// [String]
        /// SRE will ignore these types of extensions, but copy the file as is to output folder (e.g - .txt|.dat)
        /// </summary>
        ZipIgnoreFileListButCopyToOutputFolder,

        /// <summary>
        /// [Bool]
        /// 0 - simple(default) config, 1 - custom config: all the interfaces are overriden with custom interfaces
        /// </summary>
        ZipFileConfigType_not_in_use,

        /// <summary>
        /// [Bool]
        /// By default zip/tar/rar (e.g: input.zip) a acknoledgement text file will get created in output folder as &lt;file name&gt;.xml (e.g. input.zip.xml) 
        /// which will contain the unique zip id. This can be used to acknoledge that the file has been processed. 
        /// If this flag is set to boolean true, the file will not be copied to output folder
        /// </summary>
        ZipDoNotCreateAcknoledgementInOutputFolder,

        /// <summary>
        /// [Bool]
        /// Renames file column names according to associated attribute names        
        /// </summary>
        RenameColumnHeader,

        /// <summary>
        /// [String]
        /// Data feed through custom type. An array or single object can be passed directly to ProcessManager.Parse() method
        /// </summary>
        DataFeedCustomType,

        /// <summary>
        /// [bool]
        /// Whether data source has overriden 'PullLocalFileSystem' folders
        /// </summary>
        LocalFileSystemFoldersOverriden,

        /// <summary>
        /// [String]
        /// Pull folder, when data source wants to override 'PullLocalFileSystem'
        /// </summary>
        LocalFileSystemFolderPull,

        /// <summary>
        /// [String]
        /// Archive folder, when data source wants to override 'PullLocalFileSystem'
        /// </summary>
        LocalFileSystemFolderArchive,

        /// <summary>
        /// [String]
        /// Output folder, when data source wants to override 'PullLocalFileSystem'
        /// </summary>
        LocalFileSystemFolderOutput,

        /// <summary>
        /// [bool]
        /// Whether to create auto archive folder (under pull folder), when data source wants to override 'PullLocalFileSystem'
        /// </summary>
        LocalFileSystemFolderArchiveAuto,

        /// <summary>
        /// [String]
        /// Output file extension(visual),example-.txt,.csv,.data,etc.If not defined it will be .xml by default
        /// </summary>
        OutputFileExtension,

        /// <summary>
        /// [bool]
        /// If true, all wcf calls output will be written on standard output folder (useful for debugging)
        /// </summary>
        WcfCallsGenerateStandardOutput,

        /// <summary>
        /// [bool]
        /// If true returns empty string in case of error, else generic message with exception identifier (useful for debugging)
        /// </summary>
        DoNotGenerateErroredOutputWhenException,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as header1 and will be mapped to the configured attribute.
        /// </summary>
        HeaderLine1Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as header2 and will be mapped to the configured attribute.
        /// </summary>
        HeaderLine2Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as header3 and will be mapped to the configured attribute.
        /// </summary>
        HeaderLine3Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as header4 and will be mapped to the configured attribute.
        /// </summary>
        HeaderLine4Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as header5 and will be mapped to the configured attribute.
        /// </summary>
        HeaderLine5Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as header6 and will be mapped to the configured attribute.
        /// </summary>
        HeaderLine6Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as footer1 and will be mapped to the configured attribute.
        /// </summary>
        FooterLine1Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as footer2 and will be mapped to the configured attribute.
        /// </summary>
        FooterLine2Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as footer3 and will be mapped to the configured attribute.
        /// </summary>
        FooterLine3Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as footer4 and will be mapped to the configured attribute.
        /// </summary>
        FooterLine4Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as footer5 and will be mapped to the configured attribute.
        /// </summary>
        FooterLine5Attribute,

        /// <summary>
        /// [String - Name of any associated string type attribute]
        /// SRE will be configured to make first line as footer6 and will be mapped to the configured attribute.
        /// </summary>
        FooterLine6Attribute,
        
        /// <summary>
        /// [String]
        /// Output file name (without extension). If not defined, it will be same as input file name
        /// </summary>
        OutputFileName,

        /// <summary>
        /// [Bool]
        /// If partial records are allowed while writing internal output writers
        /// </summary>
        OutputPartialRecordsAllowed,

        /// <summary>
        /// [String]
        /// Output delimiter when using internal delimiter output writer
        /// </summary>
        OutputDelimiter,

        /// <summary>
        /// [Bool]
        /// If the first row is header when using internal delimiter output writer
        /// </summary>
        OutputIsFirstRowHeader,

         /// <summary>
        /// [String]
        /// Comma separated CC email ids
        /// </summary>
        EmailCc,

        /// <summary>
        /// [String]
        /// Comma separated BCC email ids
        /// </summary>
        EmailBcc,

        /// <summary>
        /// [Bool]
        /// if true, sends an email after processing each file
        /// </summary>
        EmailAfterFileProcessed,

        /// <summary>
        /// [Bool]
        /// if true, sends an email after processing each file and attach input file
        /// </summary>
        EmailAfterFileProcessedAttachInputFile,

        /// <summary>
        /// [String]
        /// Sql update query, used when loading mechanism is &apos;PullSql&apos;. This query is used to keep data interim(processing) state
        /// </summary>
        SqlUpdateQueryProcessing,

        /// <summary>
        /// [String]
        /// Sql update query, used when loading mechanism is &apos;PullSql&apos;. This query is used to keep data from interim state to error state
        /// </summary>
        SqlUpdateQueryRecovery,

        /// <summary>
        /// [String]
        /// Dynamic Output writer
        /// </summary>
        CSharpCodeOutputWriter,

        /// <summary>
        /// [String]
        /// EDIX12 Xslt string
        /// </summary>
        EDIX12Xslt,

        /// <summary>
        /// [String]
        /// Xslt string
        /// </summary>
        Xslt,

        /// <summary>
        /// [String]
        /// Dynamic DataTable Generator from file name or file content
        /// </summary>
        CSharpCodeGenerateTable,

        /// <summary>
        /// [XmlFeedMechanism]
        /// Xml feed mechanism (Xslt or CSharpCode or Custom Interface)
        /// </summary>
        XmlFeedMechanism,

        /// <summary>
        /// [String]
        /// &apos;Fixed Length&apos; schema for output writer.
        /// </summary>
        FixedLengthSchemaOutputWriter,
       
        /// <summary>
        /// [String]
        /// Database Output writer configuration including connection string key name, table and mapping information
        /// </summary>
        OutputWriterDatabaseConfiguration,

        /// <summary>
        /// [String]
        /// Scheduler datasource interface name
        /// </summary>
        SchedulerInterfaceName_TODO,


        /// <summary>
        /// [Bool]
        /// if true, sends an email after processing each file and attach output file
        /// </summary>
        EmailAfterFileProcessedAttachOutputFile,

        /// <summary>
        /// [string]
        /// Comma separated other files to be attached
        /// </summary>
        EmailAfterFileProcessedAttachOtherFiles,

         /// <summary>
        /// [string]
        /// Pipe separate tools action items
        /// </summary>
        CustomActions,

        /// <summary>
        /// [String]
        /// When loading mechanism is &apos;PullSQL&apos;, connection string at run time, defined through PulSqlInit rules
        /// </summary>
        PullSqlConnectionStringRunTime,
        
        /// <summary>
        /// [String]
        /// Various environments information and its root path, etc.
        /// </summary>
        Environments,

        /// <summary>
        /// [String]
        /// List of external dlls to be deployed
        /// </summary>
        AdditionalDeploymentArtifacts,

        /// <summary>
        /// [String]
        /// Test file name to process
        /// </summary>
        TestFileName,

        /// <summary>
        /// [String]
        /// Do not enclose each cell value with double quotes, applicable only when output writer is delimiter
        /// </summary>
        OutputDelimiterDoNotEncloseWithDoubleQuote,

    }

    public enum DataFeederTypes
    {
        [Description("Push-SRE as Service")]
        Push,

        [Description("Pull from FTP Server")]
        PullFtp,

        [Description("Pull from local file system")]
        PullLocalFileSystem,

        [Description("Pull from Database")]
        PullSql       

    }

    public enum DataFormatTypes
    {
        [Description("Delimited")]
        Delimited,

        [Description("Fixed Length")]
        FixedLength,

        [Description("Xml")]
        Xml,

        [Description("Spread Sheet")]
        SpreadSheet,

        [Description("Sql")]
        Sql,

        [Description("Zipped")]
        Zipped,

        [Description("EDIX12")]
        EDIX12,

        [Description("Multi Record")]
        MultiRecord,

        [Description("CSharpCode")]
        CSharpCode,

        [Description("Custom")]
        Custom = 99
    }

    public enum FileStatus
    {
        Unknown,
        Process,
        Ignore,
        IgnoreMoveToOutput
    }

    public enum PusherTypes
    {
        [Description("None")]
        None,

        [Description("Upload file to FTP")]
        Ftp,

        [Description("Execute DOS Commands")]
        DosCommands,

        [Description("Execute SQL Query")]
        SqlQuery,

        [Description("Custom Plugin")]
        Custom = 99
    }

    public enum OutputTypes
    {
        [Description("Xml")]
        Xml,

        [Description("Delimited")]
        Delimited,

        [Description("Fixed Length")]
        FixedLength,

        [Description("CSharpCode")]
        CSharpCode,

        [Description("Database")]
        Database,

        [Description("Custom")]
        Custom = 99
    }

    public enum AggregateOperationTypes
    {
        Sum,
        Average,
        Max,
        Min
    }

    public enum XmlFeedMechanism
    {
        Xslt,
        CSharpCode,
        Custom
    }
  
    public enum VersionObjectTypes
    {
        Unknown,
        Attribute,
        DataSource,
        Rule        
    }

    public enum EnvironmentServiceCommands
    {
        Unknown,
        Deploy,        
        DeploySdf,
        DeployArtifacts,
        GetLastDeploymentLog,
        Stop,        
        Restart,
        SetServiceLogonUser,
        StopSqlPuller,
        StartSqlPuller,        
        ExecuteDOSCommand,
        ProcessFile
    }
}




