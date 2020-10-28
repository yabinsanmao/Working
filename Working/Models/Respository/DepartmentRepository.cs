﻿using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;

namespace Working.Models.Respository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        IDbConnection _dbConnection;
        public DepartmentRepository(IDbConnection dbConnection, string connectionString)
        {

            dbConnection.ConnectionString = connectionString;
            _dbConnection = dbConnection;
        }
        /// <summary>
        /// 获取全部部门带父级部门
        /// </summary>
        /// <returns></returns>
        public List<FullDepartment> GetAllPDepartment()
        {
            return _dbConnection.Query<FullDepartment>("select d.*,pd.departmentname as pdepartmentname from departments as d join departments as pd on d.pdepartmentid=pd.id").ToList();

        }
        /// <summary>
        /// 查询全部部门
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAllDepartment()
        {
            return _dbConnection.Query<Department>("select * from departments").ToList();
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public bool AddDepartment(Department department)
        {
            return _dbConnection.Execute("insert into departments(departmentname,pdepartmentid) values (@departmentname,@pdepartmentid)",new { 
            departmentname=department.DepartmentName,pdepartmentid=department.PDepartmentID
            }) > 0;
        }
    }
}
