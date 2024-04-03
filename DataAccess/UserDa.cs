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
    public class UserDa
    {
        public User Getuser(Int64 wId)
        {
            User user = new User();
            using (StersDB stersDB = new StersDB())
            {
                user = stersDB.User.Where(a => a.Id == wId).FirstOrDefault();
            }
            return user;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (StersDB stersDB = new StersDB())
            {
                users = stersDB.User.ToList();
            }
            return users;
        }

        public List<User> GetUsers(long W_branch)
        {
            List<User> users = new List<User>();
            using (StersDB stersDB = new StersDB())
            {
                users = stersDB.User.Where(x=>x.BranchId== W_branch).ToList();
            }
            return users;
        }

        public List<User> GetActiveUsers(long W_branch)
        {
            List<User> users = new List<User>();
            using (StersDB stersDB = new StersDB())
            {
                users = stersDB.User.Where(x => x.BranchId == W_branch && x.UserStateLoging == true).ToList();
            }
            return users;
        }

        public void Adduser(User user,string password, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    User newuser = new User()
                    {
                        Id = user.Id,
                        BranchId = user.BranchId,
                        UserName = user.UserName,
                        PasswordUser = Helpers.PasswordHelper.GetSaltedPasswordHash(password),
                        Authorization = user.Authorization,
                        UserStateLoging = user.UserStateLoging,
                        Email=user.Email
                    };
                    stersDB.User.Add(newuser);
                    stersDB.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.InnerException.InnerException.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }


            
        }

        public void Updateuser(User W_user,string password, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic

                using (StersDB stersDB = new StersDB())
                {
                    var Original_user = stersDB.User.Where(a => a.Id == W_user.Id).FirstOrDefault();
                    if (Original_user != null)
                    {
                        //  stersDB.Entry(Original_user).CurrentValues.SetValues(W_user);
                        Original_user.UserName = W_user.UserName;
                        Original_user.BranchId = W_user.BranchId;
                     //   Original_user.PasswordUser = Helpers.PasswordHelper.GetSaltedPasswordHash(password);
                        Original_user.Authorization = W_user.Authorization;
                        Original_user.UserStateLoging = W_user.UserStateLoging;
                        Original_user.Email = W_user.Email;
                        stersDB.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.InnerException.InnerException.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }


           
        }

        public void UpdateuserPassword(User W_user, string password, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic

                using (StersDB stersDB = new StersDB())
                {
                    var Original_user = stersDB.User.Where(a => a.Id == W_user.Id).FirstOrDefault();
                    if (Original_user != null)
                    {
                        //  stersDB.Entry(Original_user).CurrentValues.SetValues(W_user);
                      //  Original_user.UserName = W_user.UserName;
                      //  Original_user.BranchId = W_user.BranchId;
                        Original_user.PasswordUser = Helpers.PasswordHelper.GetSaltedPasswordHash(password);
                      //  Original_user.Authorization = W_user.Authorization;
                      //  Original_user.UserStateLoging = W_user.UserStateLoging;
                        stersDB.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.InnerException.InnerException.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }



        }
        public void DeleteUser(User W_user, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                if (W_user != null)
                {
                    using (StersDB stersDB = new StersDB())
                    {
                        var x = stersDB.User.Find(W_user.Id);
                        if (x != null)
                        {
                            stersDB.User.Remove(x);
                            stersDB.SaveChanges();
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.InnerException.InnerException.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }


         
        }

        public DataTable GetUsersView1()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select tbl_user.Id,tbl_user.UserName,tbl_Agent.AgentName, tbl_user.[Authorization],tbl_user.UserStateLoging as Allow_Login from tbl_user
inner join tbl_Agent on tbl_User.BranchId=tbl_Agent.Id";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


        public User IsLogInInfoValid(string W_userName,string W_password, long W_branchID)
        {
            User user = new User();
            using (StersDB stersDB = new StersDB())
            {
                user = stersDB.User.Where(a => a.UserName == W_userName  && a.PasswordUser== W_password  && a.BranchId== W_branchID).FirstOrDefault();
            }
            return user;
        }

        public bool is_password_correct(long userId, string W_password)
        {
            User user = new User();
            using (StersDB stersDB = new StersDB())
            {
                user = stersDB.User.Where(a => a.Id == userId && a.PasswordUser == W_password).FirstOrDefault();
            }
            if (user != null)
            {
                if (user.Id > 0)
                { return true; }
                else
                { return false; }
              
            }
             
            
            return false;
        }


        public bool UserHaveEntries(Int64 wId)
        {
            bool haveentries = false;
            using (StersDB stersDB = new StersDB())
            {
               var userEntries=  stersDB.ClientCode.Where(x => x.UserId == wId || x.Person_in_charge_Id == wId).ToList();
                if (userEntries.Count > 0)
                { haveentries = true; }
                else { haveentries = false; }
            }
             return haveentries;
        }



    }
}
