using NLog;
using System;

namespace CloudCenter.Log
{
    /// <summary>
    /// Nlog日志帮助类
    /// Trace 包含大量的信息，例如 protocol payloads。一般仅在开发环境中启用, 仅输出不存文件。
    /// Debug  比 Trance 级别稍微粗略，一般仅在开发环境中启用, 仅输出不存文件。
    /// Info 一般在生产环境中启用。
    /// Warn 一般用于可恢复或临时性错误的非关键问题。
    /// Error 一般是异常信息。
    /// Fatal - 非常严重的错误！
    /// </summary>
    public class NLogHelper
    {
        readonly Logger logger;

        private NLogHelper(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 自定义 ${logger} (我用于区分文件夹)
        /// </summary>
        /// <param name="name"></param>
        public NLogHelper(string name) : this(NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetLogger(name))
        {
        }

        /// <summary>
        /// 默认 ${logger} (Default 文件夹下)
        /// </summary>
        public static NLogHelper Default { get; private set; }
        static NLogHelper()
        {
            Default = new NLogHelper(NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetLogger("Default"));
        }
        /// <summary>
        /// 比 Trance 级别稍微粗略，一般仅在开发环境中启用, 仅输出不存文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Debug(string msg, params object[] args)
        {
            logger.Debug(msg, args);
        }
        /// <summary>
        /// 比 Trance 级别稍微粗略，一般仅在开发环境中启用, 仅输出不存文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Debug(string msg, Exception err)
        {
            logger.Debug(err, msg);
        }
        /// <summary>
        /// 一般在生产环境中启用。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Info(string msg, params object[] args)
        {
            logger.Info(msg, args);
        }
        /// <summary>
        /// 一般在生产环境中启用。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Info(string msg, Exception err)
        {
            logger.Info(err, msg);
        }
        /// <summary>
        /// 包含大量的信息，例如 protocol payloads。一般仅在开发环境中启用, 仅输出不存文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Trace(string msg, params object[] args)
        {
            logger.Trace(msg, args);
        }
        /// <summary>
        /// 包含大量的信息，例如 protocol payloads。一般仅在开发环境中启用, 仅输出不存文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Trace(string msg, Exception err)
        {
            logger.Trace(err, msg);
        }
        /// <summary>
        /// 一般是异常信息。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Error(string msg, params object[] args)
        {
            logger.Error(msg, args);
        }
        /// <summary>
        /// 一般是异常信息。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Error(string msg, Exception err)
        {
            logger.Error(err, msg);
        }
        /// <summary>
        ///  非常严重的错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Fatal(string msg, params object[] args)
        {
            logger.Fatal(msg, args);
        }
        /// <summary>
        ///  非常严重的错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Fatal(string msg, Exception err)
        {
            logger.Fatal(err, msg);
        }
    }
}
 
