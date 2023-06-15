using StackExchange.Redis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MyProject.Util {
    public class RedisHelper {
        private static ConfigurationOptions options;
        // 设置编码，否则中文乱码
        private static JsonSerializerOptions encoder = new JsonSerializerOptions() {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
        };
        static RedisHelper() {
            options = new ConfigurationOptions();

            // 读取配置文件
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json").Build();
            int timeout = int.Parse(configuration["Redis:timeout"]);
            string server = configuration["Redis:server"];
            string port = configuration["Redis:port"];
            string password = configuration["Redis:password"];
            /* options.ConnectTimeout = 5000; // 设置超时时间
             options.EndPoints.Add("192.168.26.129:6379"); // 设置IP和端口
             options.Password = "123456"; // 密码*/
            options.ConnectTimeout = timeout;
            options.EndPoints.Add(server + ":" +  port);
            options.Password = password;
        }

        /// <summary>
        /// 以string的方式存入到Redis中
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="Object">对象的值</param>
        /// <returns>true表示存储成功</returns>
        public bool setString<T>(string key, T Object) {
            // 先将object转换成json
            string json = transfeToJson(Object);
            if(json == "") {
                // 转换失败，直接返回false
                return false;
            }
            // 获取Redis的操作对象
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                // 获取操作的数据库对象
                IDatabase db = conn.GetDatabase();
                // 将json存入到Redis
                return db.StringSet(key, json);
            }
          
        }

        /// <summary>
        /// 以string的方式存入到Redis中并且设置过期时间
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="Object">对象的值</param>
        /// <param name="seconds">过期时间，单位为秒</param>
        /// <returns>true表示存储成功</returns>
        public bool setString<T>(string key, T Object, int seconds) {
            // 先将object转换成json
            string json = transfeToJson(Object);
            if (json == "") {
                // 转换失败，直接返回false
                return false;
            }
            // 获取Redis的操作对象
            using (var conn = ConnectionMultiplexer.Connect(options)) {
                // 获取操作的数据库对象
                IDatabase db = conn.GetDatabase();
                // 将json存入到Redis
                return db.StringSet(key, json,TimeSpan.FromSeconds(seconds));
            }
        }

        /// <summary>
        /// 获取string类型的key对应的object对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>key对应存在返回对象，不存在返回空</returns>
        public T getStringObject<T> (string key) {
            string json = getString(key);
            if(json == "") {
                return default(T);
            }
            else {
               return transfeToObject<T>(json);
            }
        }

        /// <summary>
        /// 获取key对应的json
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>返回空串表示不存在</returns>
        public string getString(string key) {
          
            // 操作redis
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                // 获取数据库
                IDatabase db = conn.GetDatabase();
                // key存在则返回
                if(exist(key)) {
                    return db.StringGet(key);
                }
                else
                {
                    // 不存在返回空串
                    return "";
                }

            }
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>存在返回true</returns>
        public bool exist(string key) {
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                return db.KeyExists(key);
            }
        }

        /// <summary>
        /// 设置key的过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">过期时间，单位为秒</param>
        /// <returns>设置成功返回true</returns>
        public bool setExpire(string key, int seconds) {

            if(!exist(key)) {
                return false;
            }
           using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                // 设置过期时间
                return db.KeyExpire(key,TimeSpan.FromSeconds(seconds));
                
            }
        }

        /// <summary>
        /// 将objectList插入到List列表中
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">list的key</param>
        /// <param name="objectList">对象集合</param>
        /// <returns>true表示插入成功</returns>
        public bool listLeftPushAll<T>(string key, List<T> objectList) {
            // 先将objectList转换成jsonList
           List<string> stringList = transfeToStringList(objectList);
            if(stringList == null) {
                return false;
            }
            // 将jsonList插入到Redis中
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                long before = db.ListLength(key);
                foreach (string json in stringList) {
                    db.ListLeftPush(key, json);
                }
                long after = db.ListLength(key);
                return !(before == after);
            }
        }

        /// <summary>
        /// 向List中leftPush元素
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="Object">对象的值</param>
        /// <returns>日否插入成功</returns>
        public bool listLeftPush<T>(string key, T Object) {
            // 先将object转换成json
           string json = transfeToJson(Object);
            if(json == "") {
                return false;
            }
            // 将json插入到List中
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                // 获取插入前的长度
              long before =  db.ListLength(key);
               long after = db.ListLeftPush(key, json);
                return !(before == after);
            }
        }

        /// <summary>
        /// 将objectList插入到List列表中
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">list的key</param>
        /// <param name="objectList">对象集合</param>
        /// <returns>true表示插入成功</returns>
        public bool listRightPushAll<T>(string key, List<T> objectList) {
            // 先将objectList转换成jsonList
            List<string> stringList = transfeToStringList(objectList);
            if (stringList == null) {
                return false;
            }
            // 将jsonList插入到Redis中
            using (var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                long before = db.ListLength(key);
                foreach (string json in stringList) {
                    db.ListRightPush(key, json);
                }
                long after = db.ListLength(key);
                return !(before == after);
            }
        }

        /// <summary>
        /// 向List中rightPush元素
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="Object">对象的值</param>
        /// <returns>日否插入成功</returns>
        public bool listRightPush<T>(string key, T Object) {
            // 先将object转换成json
            string json = transfeToJson(Object);
            if (json == "") {
                return false;
            }
            // 将json插入到List中
            using (var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                // 获取插入前的长度
                long before = db.ListLength(key);
                long after = db.ListRightPush(key, json);
                return !(before == after);
            }
        }

        /// <summary>
        /// 从List左边移除并返回第一个对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">List对应的key</param>
        /// <returns>对象存在返回对象的值，否则返回null</returns>
        public T getListLeftPop<T> (string key) {
            // key不存在返回空
            if(!exist(key)) {
                return default(T);
            }
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                     // 获取Redisvalue
                    RedisValue value =  db.ListLeftPop(key);
                // 将RedisValue转换成T类型的对象
              return  transfeToObject<T>(value.ToString());
            }

        }


        /// <summary>
        /// 从List右边移除并返回第一个对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">List对应的key</param>
        /// <returns>对象存在返回对象的值，否则返回null</returns>
        public T getListRightPop<T> (string key) {
            // key不存在返回空
            if (!exist(key)) {
                return default(T);
            }
            using (var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                // 获取Redisvalue
                RedisValue value = db.ListRightPop(key);
                // 将RedisValue转换成T类型的对象
                return transfeToObject<T>(value.ToString());
            }
        }

        /// <summary>
        /// 返回从start到end的对象的集合
        /// start = 0,end = -1表示返回所有的元素
        /// start = 0, end = 1表示返回下标为0到1的元素，包括1
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="start">开始下标，默认值0</param>
        /// <param name="end">结束下标，默认值-1</param>
        /// <returns>对象的集合</returns>
        public List<T> getListRange<T>(string key, int start = 0, int end = -1){
            List<T> objectList = new List<T>();
            if(!exist(key)) {
                return objectList;
            }
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                // 获取stringList
                 var stringList = db.ListRange(key, start, end);
               objectList = transfeToObjectList<T>(stringList);
            }
            return objectList;
        }

        /// <summary>
        /// 返回key对应的list的长度
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>list的长度</returns>
        public long getListLength(string key) {
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
                return  db.ListLength(key);
            }
        }


        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>true表示删除成功</returns>
        public bool deleteKey(string key) {
            if(!exist(key)) {
                return false;
            }
            using(var conn = ConnectionMultiplexer.Connect(options)) {
                IDatabase db = conn.GetDatabase();
               return db.KeyDelete(key);
            }
        }


        /// <summary>
        /// 将json集合转换成object集合
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="stringList">json集合</param>
        /// <returns>转换后的对象集合</returns>
        private List<T> transfeToObjectList<T>(RedisValue[] stringList) {
            List<T> objectList = new List<T>();

            try {
                for (int i = 0; i < stringList.Length; i++) {
                    string json = stringList[i].ToString();
                    objectList.Add(JsonSerializer.Deserialize<T>(json));
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return objectList;
        }

        /// <summary>
        /// 将objectList转换成stringList
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="objectList">对象集合</param>
        /// <returns>字符串集合</returns>
        private List<string> transfeToStringList<T>(List<T> objectList) {
            List<string> list = new List<string>();
            try {
                foreach (T Object in objectList) {
                    list.Add(JsonSerializer.Serialize(Object, encoder));
                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
      
            return list;
        }


        /// <summary>
        /// 将对象转换成json字符串
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="Object">对象的值</param>
        /// <returns>转换后的json字符串</returns>
        private  string transfeToJson<T>(T Object) {
            string json = "";
            try {
                json = JsonSerializer.Serialize(Object, encoder);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return json;
        }

        /// <summary>
        /// 将json字符串转换成对应的object对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>转换后的object对象</returns>
        private T transfeToObject<T> (string json) {
            try {
                return JsonSerializer.Deserialize<T>(json);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }

            return default(T);
        }

    }
}
