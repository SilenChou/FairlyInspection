using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FairlyInspection.Models
{
    /// <summary>
    /// 1.系統管理員  2.後台管理員
    /// </summary>
    public enum AdminType //管理員類型
    {
        /// <summary>
        /// 系統管理員
        /// </summary>
        [Description("系統管理員")]
        Sysadmin = 1,

        /// <summary>
        /// 總經理
        /// </summary>
        [Description("總經理")]
        President = 2,

        /// <summary>
        /// 副總
        /// </summary>
        [Description("副總")]
        VicePresident = 3,

        /// <summary>
        /// 經理
        /// </summary>
        [Description("經理")]
        Manager = 4,

        /// <summary>
        /// 調度員
        /// </summary>
        [Description("調度員")]
        Dispatcher = 5,

        /// <summary>
        /// 巡檢人員
        /// </summary>
        [Description("巡檢人員")]
        Inspector = 6,

        /// <summary>
        /// 課長
        /// </summary>
        [Description("課長")]
        Chief = 7,

        /// <summary>
        /// 申請人
        /// </summary>
        [Description("申請人")]
        Applicant = 9,
    }

    /// <summary>
    /// 1.系統管理員  2.後台管理員
    /// </summary>
    public enum MeetAdminType //帳號類型
    {
        /// <summary>
        /// 系統管理員
        /// </summary>
        [Description("系統管理員")]
        Sysadmin = 1,

        /// <summary>
        /// 總經理
        /// </summary>
        [Description("總經理")]
        President = 2,

        /// <summary>
        /// 副總
        /// </summary>
        [Description("副總")]
        Vice = 3,

        /// <summary>
        /// 主管
        /// </summary>
        [Description("主管")]
        Manager = 5,

        /// <summary>
        /// 課長
        /// </summary>
        [Description("課長")]
        Chief = 6,

        /// <summary>
        /// 一般員工
        /// </summary>
        [Description("一般員工")]
        Employee = 7,
    }

    /// <summary>
    /// 廠區
    /// </summary>
    public enum Factory
    {
        /// <summary>
        /// 一廠
        /// </summary>
        [Description("一廠")]
        Bar = 1,

        /// <summary>
        /// 二廠
        /// </summary>
        [Description("二廠")]
        Straight = 2,

        /// <summary>
        /// 三廠
        /// </summary>
        [Description("三廠")]
        Square = 3,
    }

    /// <summary>
    /// 層級 1.廠區  2.設備系統 3.機器 4.組件 5.零件 6.細部零件
    /// </summary>
    public enum LevelClass
    {
        /// <summary>
        /// 菲力工業
        /// </summary>
        [Description("菲力工業")]
        Fairlybike = 0,
        /// <summary>
        /// 廠區
        /// </summary>
        [Description("廠區")]
        Factory = 1,

        /// <summary>
        /// 設備系統
        /// </summary>
        [Description("設備系統")]
        System = 2,

        /// <summary>
        /// 機器
        /// </summary>
        [Description("機器")]
        Machine = 3,

        /// <summary>
        /// 組件
        /// </summary>
        [Description("組件")]
        Components = 4,

        /// <summary>
        /// 零件
        /// </summary>
        [Description("零件")]
        Parts = 5,

        /// <summary>
        /// 細部零件
        /// </summary>
        [Description("細部零件")]
        Detail = 6,

    }

    /// <summary>
    /// 廠商類型
    /// </summary>
    public enum ManufactureType
    {
        /// <summary>
        /// 類型一
        /// </summary>
        [Description("類型一")]
        Failed = 1,

        /// <summary>
        /// 類型二
        /// </summary>
        [Description("類型二")]
        NotSignable = 2,

        /// <summary>
        /// 類型三
        /// </summary>
        [Description("類型三")]
        NotInList = 3,
    }

    /// <summary>
    /// 檢查結果 
    /// 1.正常  2.故障/缺損 3.遺失 4.其他
    /// </summary>
    public enum LogCheckResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,

        /// <summary>
        /// 故障/缺損
        /// </summary>
        [Description("故障/缺損")]
        Defect = 2,

        /// <summary>
        /// 遺失
        /// </summary>
        [Description("遺失")]
        Missing = 3,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Others = 4,
    }

    /// <summary>
    /// 計時單位 
    /// 1.年  2.月 3.日
    /// </summary>
    public enum TimeUnit
    {
        /// <summary>
        /// 年
        /// </summary>
        [Description("年")]
        Year = 1,

        /// <summary>
        /// 月
        /// </summary>
        [Description("月")]
        Month = 2,

        /// <summary>
        /// 日
        /// </summary>
        [Description("日")]
        Day = 3,
    }

}