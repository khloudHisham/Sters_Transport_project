using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StersTransport.Models;

namespace StersTransport.DataAccess
{
    public class BranchDa
    {
        public Branch GetBranch(Int64 wId)
        {
            Branch branch = new Branch();
            using (StersDB stersDB = new StersDB())
            {
                branch = stersDB.Branch.Where(a => a.Id == wId).FirstOrDefault();
            }
            return branch;
        }
        public List<Branch> GetBranches()
        {
            List<Branch> branches = new List<Branch>();
            using (StersDB stersDB = new StersDB())
            {
                branches = stersDB.Branch.ToList();
            }
            return branches;
        }

        public DataTable GetBranchesView1()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select tbl_Branch.Id,tbl_City.CityName, BranchName,ContactPersonName,BranchCompany,PhoneNo1,PhoneNo2,Address from tbl_Branch  inner join tbl_City
on tbl_Branch.CityId=tbl_City.Id";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void Addbranch(Branch branch, out string errormessage)
        {
            errormessage = string.Empty;

            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    Branch newbranch = new Branch()
                    {
                        Id = branch.Id,
                        BranchName = branch.BranchName,
                        ContactPersonName = branch.ContactPersonName,
                        BranchCompany = branch.BranchCompany,
                        PhoneNo1 = branch.PhoneNo1,
                        PhoneNo2 = branch.PhoneNo2,
                        Address = branch.Address,
                        CityId = branch.CityId,
                        CharactersPrefix = branch.CharactersPrefix,
                        YearPrefix = branch.YearPrefix,
                        NumberOfDigits = branch.NumberOfDigits,
                        InvoiceLanguage = branch.InvoiceLanguage,
                        PhonesDisplayString = branch.PhonesDisplayString,
                        IsLocalCompanyBranch = branch.IsLocalCompanyBranch
                    };
                    stersDB.Branch.Add(newbranch);
                    stersDB.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }

          
        }
        public void UpdateBranch(Branch W_branch, out string errormessage)
        {

            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    var Original_branch = stersDB.Branch.Where(a => a.Id == W_branch.Id).FirstOrDefault();
                    if (Original_branch != null)
                    {
                        stersDB.Entry(Original_branch).CurrentValues.SetValues(W_branch);
                        stersDB.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }



           
        }

        public void DeleteBranch(Branch W_branch, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic

                if (W_branch != null)
                {
                    using (StersDB stersDB = new StersDB())
                    {
                        var x = stersDB.Branch.Find(W_branch.Id);
                        if (x != null)
                        {
                            stersDB.Branch.Remove(x);
                            stersDB.SaveChanges();
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }



        }


        public List<ClientCode> getcodeEntries(Branch W_branch)
        {
            List<ClientCode> codes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                codes = stersDB.ClientCode.Where(x => x.BranchId == W_branch.Id).ToList();
            }
            return codes;
        }

        public bool isNewNumberOfDigitsValid(Branch w_branch)
        {
            long? newNUmber = w_branch.NumberOfDigits;
            Branch originalBranch = GetBranch(w_branch.Id);
            long? originalNUmber = originalBranch.NumberOfDigits;

            bool isvalid = false;
            
            if (newNUmber < originalNUmber)
            {
                isvalid = false;
            }
            else
            {
                isvalid = true;
            }
            return isvalid;
        }



    }
}
