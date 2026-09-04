using ARMCommon.Model;
using AxExtend.Interface;
using AxiApi.DTOs;
using AxiApi.Exceptions;
using AxiApi.Interfaces;
using System.Data;
using System.Threading.Tasks;

namespace AxiApi.Repositories
{
    public class UserFavouritesRepository: IUserFavouritesRepository
    {
        private readonly ILogger<GrammarRepository> _logger;
        private readonly IAxExtend _axExtend;

        public UserFavouritesRepository(IAxExtend axExtend, ILogger<GrammarRepository> logger)
        {
            //_connectionString = configuration.GetConnectionString("Postgres")!;

            _axExtend = axExtend;
            _logger = logger;
        }

        public async Task<List<UserFavouritesDTO>> CreateUserFavourites(UserFavouritesDTO userFavouritesDTO, string appname)
        {
            // userFavouritesDTO.FavouritesId = Guid.NewGuid();
            string sqlQuery = "INSERT INTO Axi_UserFavourites\r\n(username, commandtext,originalcommandtext, favorder, targeturl)\r\nVALUES\r\n(:username, :commandtext,:originalcommandtext, :favorder, :targeturl);";

            string selectSqlQuery = "SELECT * FROM Axi_UserFavourites WHERE originalcommandtext = :originalcommandtext"; 
            string[] paramNames = {":username", ":commandtext", ":originalcommandtext", ":favorder", ":targeturl" };
            DbType[] paramTypes = { DbType.String, DbType.String, DbType.String, DbType.Int64, DbType.String };
            object[] paramValues = {userFavouritesDTO.Username, userFavouritesDTO.CommandText, userFavouritesDTO.OriginalCommandText, userFavouritesDTO.FavOrder, userFavouritesDTO.TargetURL };


            string[] selectParamNames = { ":originalcommandtext" };
            DbType[] selectParamTypes = { DbType.String };
            object[] selectParamValues = { userFavouritesDTO.OriginalCommandText }; 

            NonQueryResult nonQueryResult = new();
            SQLResult sqlResult = new(); 
            

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);

            if (isDbConnected)
            {
                var db = await _axExtend.GetDB();

                nonQueryResult = await db.ExecuteNonQueryAsync(sqlQuery, paramNames, paramTypes, paramValues);
                  if (!string.IsNullOrEmpty(nonQueryResult?.error))
                        {
                           throw new DatabaseException(nonQueryResult.error,"INSERT");

                        }
                sqlResult = await db.ExecuteSQLAsync(selectSqlQuery,selectParamNames, selectParamTypes, selectParamValues); 

                 if (!string.IsNullOrEmpty(sqlResult?.error))
                        {
                           throw new DatabaseException(sqlResult.error,"SELECT");

                        }



               
            }

            List<UserFavouritesDTO> userFavouritesDTOs = new(); 

            if (sqlResult != null && sqlResult.data != null && sqlResult.data.Rows.Count > 0)
            {
                foreach (DataRow row in sqlResult.data.Rows)
                {
                    var fav = new UserFavouritesDTO
                    {
                        FavouritesId = row["id"] is DBNull ? Guid.Empty : Guid.Parse(row["id"].ToString()),
                        Username = row["username"]?.ToString(),
                        CommandText = row["commandtext"]?.ToString(),
                        OriginalCommandText = row["originalcommandtext"]?.ToString(),
                        FavOrder = row["favorder"] is DBNull ? 0 : Convert.ToInt32(row["favorder"]),
                        TargetURL = row["targeturl"]?.ToString(),
                        CreatedOn = row["createdon"] is DBNull ? DateTime.MinValue : (DateTime)row["createdon"],

                    };

                    userFavouritesDTOs.Add(fav);
                }
            }

            return userFavouritesDTOs; 




        }

        public async Task<NonQueryResult> DeleteUserFavouritesByCmd(UserFavouritesDTO userFavouritesDTO, string appname)
        {
            string sqlQuery = "DELETE FROM Axi_UserFavourites WHERE username = :username AND commandtext = :commandtext;";

            string[] paramNames = { ":username", ":commandtext" };
            DbType[] paramTypes = { DbType.String, DbType.String };
            object[] paramValues = { userFavouritesDTO.Username, userFavouritesDTO.CommandText };

            NonQueryResult sqlResult = new();


            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);

            if (isDbConnected)
            {
                var db = await _axExtend.GetDB();

                sqlResult = await db.ExecuteNonQueryAsync(sqlQuery, paramNames, paramTypes, paramValues);


            }

            return sqlResult;

        }

        public async Task<List<UserFavouritesDTO>> GetUserFavouritesByUsername(string username, string appname)
        {
            string sqlQuery = "SELECT * FROM Axi_UserFavourites WHERE username = :username ORDER BY createdon DESC";

            string[] paramNames = { ":username" };
            DbType[] paramTypes = { DbType.String };
            object[] paramValues = { username };

            SQLResult sqlResult = new();
            List<UserFavouritesDTO> favouritesDTOs = new(); 

            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname); 

            if (isDbConnected)
            {
                var db = await _axExtend.GetDB();



                sqlResult = await db.ExecuteSQLAsync(sqlQuery, paramNames, paramTypes, paramValues);
            }


            if (sqlResult != null && sqlResult.data != null && sqlResult.data.Rows.Count > 0)
            {
                foreach (DataRow row in sqlResult.data.Rows)
                {
                    var fav = new UserFavouritesDTO
                    {
                        FavouritesId = row["id"] is DBNull ? Guid.Empty : Guid.Parse(row["id"].ToString()),
                        Username = row["username"]?.ToString(),
                        CommandText = row["commandtext"]?.ToString(),
                        OriginalCommandText = row["originalcommandtext"]?.ToString(),
                        FavOrder = row["favorder"] is DBNull ? 0 : Convert.ToInt32(row["favorder"]),
                        TargetURL  = row["targeturl"]?.ToString(),
                        CreatedOn = row["createdon"] is DBNull ? DateTime.MinValue : (DateTime)row["createdon"],
                        
                    };

                    favouritesDTOs.Add(fav);
                }
            }

            return favouritesDTOs; 



        }

        public async Task<NonQueryResult> UpdateUserFavourties(string favouritesId, string appname, string commandText)
        {

            // string sqlQuery = "UPDATE Axi_UserFavourites SET commandtext = :commandtext WHERE id = :favouritesid;";
            string sqlQuery = $"UPDATE Axi_UserFavourites SET commandtext = '{commandText}' WHERE id = '{favouritesId}'";

            // string[] paramNames = { ":favouritesid", ":commandtext" };
            // DbType[] paramTypes = { DbType.String, DbType.String };
            // object[] paramValues = { favouritesId,commandText };

            NonQueryResult sqlResult = new();


            var isDbConnected = await _axExtend.OpenDBConnectionAsync(appname);

            if (isDbConnected)
            {
                var db = await _axExtend.GetDB();

                // sqlResult = await db.ExecuteNonQueryAsync(sqlQuery, paramNames, paramTypes, paramValues);
                sqlResult = await db.ExecuteNonQueryAsync(sqlQuery);


            }

            return sqlResult;



        }

      
    }
}
