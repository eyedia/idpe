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

namespace Eyedia.IDPE.Common
{
    public enum IdpeMessageCodes
    {
        //IDPE Engine
        IDPE_SUCCESS = 100,
        IDPE_FAILED = 101,
        IDPE_UNKNOWN_ERROR = 103,
        IDPE_FAILED_BLANK = 104,


        //IDPEType (generic)
        IDPE_TYPE_DATA_VALIDATION_FAILED_GENERIC = 200,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MINIMUM = 201,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM = 202,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_STRING = 203,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_STRING = 204,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_DATETIME_SERVER = 205,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_DATETIME_SERVER = 206,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_DATE_SERVER = 207,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MAXIMUM_DATE_SERVER = 208,
        IDPE_TYPE_DATA_VALIDATION_FAILED_MINIMUM_MAXIMUM_DATE = 209,
        IDPE_TYPE_DATA_VALIDATION_FAILED_VALUE_UPDATION_NOT_PERMITTED = 210,        
        

        //IDPEType specific
        IDPE_INT_TYPE_DATA_VALIDATION_FAILED = 301,
        IDPE_BIGINT_TYPE_DATA_VALIDATION_FAILED = 302,
        IDPE_DECIMAL_TYPE_DATA_VALIDATION_FAILED = 303,
        IDPE_BIT_TYPE_DATA_VALIDATION_FAILED = 304,
        IDPE_STRING_TYPE_DATA_VALIDATION_FAILED = 305,
        IDPE_REFERENCED_TYPE_DATA_RESULT_FOUND = 306,
        IDPE_REFERENCED_TYPE_DATA_RESULT_NOT_FOUND = 307,
        IDPE_REFERENCED_TYPE_DATA_CAN_NOT_BE_NULL = 308,
        IDPE_NOT_REFERENCED_TYPE_DATA_RESULT_FOUND = 309,
        IDPE_NOT_REFERENCED_TYPE_DATA_RESULT_NOT_FOUND = 310,
        IDPE_DATE_TYPE_DATA_VALIDATION_FAILED = 311,
        IDPE_GENERATED_TYPE_DATA_VALIDATION_FAILED = 312,
        IDPE_CODESET_TYPE_DATA_VALIDATION_FAILED = 313,

        //Plugin specific
        IDPE_PLUGIN_METHOD_EXECUTION_FAILED = 501,
        IDPE_PLUGIN_METHOD_EXECUTION_FAILED_METHOD_NOT_DEFINED = 502,

        //attribute
        IDPE_ATTRIBUTE_ASSIGN_FAILED = 601,

        //container
        IDPE_CONTAINER_VALIDATION_FAILED = 701,
        IDPE_CONTAINER_VALIDATION_FAILED_ROW = 702,

        //file types
        IDPE_FILE_GENERIC = 801,
        IDPE_FILE_TYPE_NOT_SUPPORTED = 802
    }
}




