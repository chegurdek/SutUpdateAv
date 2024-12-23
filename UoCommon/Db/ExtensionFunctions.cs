﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Text;
using Elsy.UoCommon.Models;


namespace Elsy.UoCommon.Db
{
    public static  class ExtensionFunctions
    {
        public static DbParameter [] ToParamerArray(this Parameter[] parameters, DbCommand cmd)
        {
            DbParameter[] sp = new DbParameter[parameters.Length];
            int i = 0;
            foreach (Parameter parameter in parameters)
            {
                // DbParameter p = cmd.CreateParameter();
                sp[i] = cmd.CreateParameter();
                sp[i].ParameterName = parameter.ParameterName;
                sp[i].Value = parameter.Value;
                sp[i].Direction = string.IsNullOrEmpty(Convert.ToString(parameter.Direction)) || 
                    parameter.Direction == 0 ? ParameterDirection.Input : parameter.Direction;
                sp[i].DbType = parameter.DbType;
                sp[i].SourceColumn = parameter.SourceColumn;
                sp[i].Size = parameter.Size;
                i++;
            }
            return sp;
        }
    }
}
