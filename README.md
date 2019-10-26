# Ocph.DAL.MySql

Welcome to the OcphDAL for Simple  Mysql Data Access Layer !
1. Install From Nuget
   https://www.nuget.org/packages/Ocph.DAL.MySql/
2. Crete Connection
   1. Add Connection Config
      Exp. 
              
                <connectionStrings>
               <add name="DefaultConnection" providerName="MySql.Data.MySqlClient" 
                  connectionString="Server=localhost;database=plagiatdb;UID=root;password=;CharSet=utf8;Persist Security 
                Info=True" />
               </connectionStrings>


   2. When Use DbContext
      .......
      


3. Create Model  Example :

           namespace Models.Data
               {
                [TableName("tb_rootword")]
                public class RootWord
                 {
                   [PrimaryKey("Id")]
                   [DbColumn("id_ktdasar")]
                   public int Id { get; set; }

                  [DbColumn("rootword")]
                  public string Word { get; set; }

                 [DbColumn("tipe_katadasar")]
                 public string TypeRootWord { get; set; }
                  }
            }

            Or Use AppDatabaseModelCreator (See Code)

   
4. Create Repository
   ***
   Inherit from IDbContext

         public class OcphDbContext : IDbContext, IDisposable
       {
        private string ConnectionString;
        private IDbConnection _Connection;

        public OcphDbContext(string constring)
        {

            this.ConnectionString = constring;
        }

        public OcphDbContext()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
       
        internal IRepository<RootWord> RoadWords { get { return new Repository<RootWord>(this); } }
     
        public IDbConnection Connection
        {
            get
            {
                if (_Connection == null)
                {
                    _Connection = new MySqlDbContext(this.ConnectionString);
                    return _Connection;
                }
                else
                {
                    return _Connection;
                }
            }
        }


        public void Dispose()
        {
            if (_Connection != null)
            {
                if (this.Connection.State != ConnectionState.Closed)
                {
                    this.Connection.Close();
                }
            }
        }
        }
***

5. Query
   1. Select


              using(var db = new OcphDbContext())
               {
                  // Get All Data
                  var rootWords = db.RootWords.Select();
                      //With Linq 
                      var rootWors = from data in db.RootWords.Select()
                                     select data;
                  // with Clause Where
                   var roadWords = db.RoadWords.Where(O=>O.TypeRootWord=='Noun');
                        //With Linq
                         var rootWors = from data in db.RootWords.Select() where data.TypeRootWord equal 'Noun'
                                     select data;

                  // You Can Join Like Linq
                   var data = from a in db.TableA  
                              join b in db.TableB on a.Id equal b.Ida
                              select b;
               }

   2. Insert
                 
               using(var db = new OcphDbContext())
               {
                    var saved = db.RootWords.Insert(rootWord);
                    var Id = db.RootWords.InsertWithGetLastId(rootWord);
                 }

                //With Transaction
                   using(var db = new OcphDbContext())
               {
                     var trans= db.Connection.BeginTransaction();
                   try
                     {
                           var Id = db.RootWords.InsertWithGetLastId(rootWord);
                           var item1= new Item();
                           item1.Id=Id;
                            var saved = db.Items.Insert(item1);
                            trans.Commit();
                      }catch(MySqlExecption ex)
                      {
                           trans.Rollback();   
                      }
                    
                 }
                  
   3. Delete


               using(var db = new OcphDbContext())
               {
                    var isDeleted = db.RootWords.Delete(O=>O.Id==1);
                 
                 }

    
   4. Update


               using(var db = new OcphDbContext())
               {
                    var isUpdated = db.RootWords.Update(O=> new{O.Word},rootWordToUpdate,O=>O.Id==rootWordToUpdate );
                 
                 }


***
