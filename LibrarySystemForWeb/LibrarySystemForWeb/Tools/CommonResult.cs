using System;

namespace LibrarySystemForWeb.Tools
{
    public class CommonResult
    {
        //描述统一格式中的数据
        public object data { get; set; }

        //描述统一格式中的编码，用于区分操作，可以简化配置0或1表示成功失败
        public int code { get; set; }

        //描述统一格式中的消息，可选属性
        public string msg { get; set; }
        // 
        public int count { get; set; }

        public CommonResult()
        {
        }
        public CommonResult(int code)
        {
            this.code = code;
        }

        public CommonResult(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }


        public CommonResult(int code, object data)
        {
            this.data = data;
            this.code = code;
        }
        public CommonResult(int code, object data, int count)
        {
            this.count = count;
            this.data = data;
            this.code = code;
        }


        public CommonResult(int code, object data, string msg)
        {
            this.data = data;
            this.code = code;
            this.msg = msg;
        }

 
        public CommonResult(int code, object data, string msg, int count)
        {
            this.data = data;
            this.code = code;
            this.msg = msg;
            this.count = count;
        }

        public static CommonResult Success()
        {
            return new CommonResult(200);
        }

        public static CommonResult Success(object data)
        {
            return new CommonResult(200,data);
        }

        public static CommonResult Success(string msg)
        {
            return new CommonResult(200, msg);
        }

        public static CommonResult Success(object data, int count)
        {
            return new CommonResult(200, data, count);
        }

        public static CommonResult Success(object data, string msg)
        {
            return new CommonResult(200, data, msg);
        }

        public static CommonResult Failed(string msg)
        {
            return new CommonResult(555,msg);
        }

        public static CommonResult Failed()
        {
            return new CommonResult(500);
        }

    }
}