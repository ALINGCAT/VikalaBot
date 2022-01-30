using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Models.Enumeration;
using OneBot.CommandRoute.Services;
using OneBot.FrameworkDemo.Attributes;
using OneBot.FrameworkDemo.Models;
using Sora.Entities;
using Sora.Entities.Segment;
using Sora.EventArgs.SoraEvent;
using VikalaBot.Database;
using VikalaBot.Database.Entities;

namespace VikalaBot.Modules
{
    public class GBFModule : IOneBotController
    {
        internal enum DrawInfoType { Crystal, Draw, DrawTen }
        private readonly ILogger<GBFModule> _logger;

        public GBFModule(ICommandService commandService, ILogger<GBFModule> logger)
        {
            commandService.Event.OnGroupRequest += (context) =>
            {
                var args = context.WrapSoraEventArgs<AddGroupRequestEventArgs>();
                args.Accept();
                return 1;
            };
            // 全局异常处理事件
            commandService.Event.OnException += (context, exception) =>
            {
                logger.LogError($"{exception.Message}");
            };

            _logger = logger;
        }

        [Command("crystal <count>", Alias = new[] { "!宝晶石 <count>", "!bjs <count>" }, EventType = EventType.GM)]
        public void SetCrystal(int count, GroupMessageEventArgs e, VikalaDbContext db)
        {
            if (SetDrawInfo(db, e.Sender.Id, DrawInfoType.Crystal, count))
                e.Reply(SoraSegment.At(e.Sender.Id) + "宝晶石记录成功!");
        }

        [Command("crystal <count>", Alias = new[] { "!单抽 <count>", "!单抽券 <count>", "!单抽卷 <count>" }, EventType = EventType.GM)]
        public void SetDraw(int count, GroupMessageEventArgs e, VikalaDbContext db)
        {
            if (SetDrawInfo(db, e.Sender.Id, DrawInfoType.Draw, count))
                e.Reply(SoraSegment.At(e.Sender.Id) + "单抽券记录成功!");
        }

        [Command("crystal <count>", Alias = new[] { "!十连券 <count>", "!十连卷 <count>", "!十连 <count>" }, EventType = EventType.GM)]
        public void SetDrawTen(int count, GroupMessageEventArgs e, VikalaDbContext db)
        {
            if (SetDrawInfo(db, e.Sender.Id, DrawInfoType.DrawTen, count))
                e.Reply(SoraSegment.At(e.Sender.Id) + "十连券记录成功!");
        }

        [Command("well", Alias = new[] { "!算井" }, EventType = EventType.GM)]
        public void Well(GroupMessageEventArgs e, VikalaDbContext db)
        {
            var info = db.DrawInfos.FirstOrDefault(i => i.QQ == e.Sender.Id);
            if (info == null)
                e.Reply(SoraSegment.At(e.Sender.Id) +
                    "未找到您的数据,请检查是否已录入过宝晶石等库存信息!");
            else
            {
                var times = info.Crystal / 300 + info.Draw + info.DrawTen * 10;
                e.Reply(SoraSegment.At(e.Sender.Id) +
                    $"根据已录入的数据:\n" +
                    $"宝晶石: { info.Crystal } -> { info.Crystal % 300 } 共{ info.Crystal / 300 }抽\n" +
                    $"单抽:{ info.Draw }张 十连券:{ info.DrawTen }张\n" +
                    $"总计{ times }抽 距离一井还差{ 300 - times }抽");
            }
        }

        internal bool SetDrawInfo(VikalaDbContext db, long qq, DrawInfoType type, int count)
        {
            var info = db.DrawInfos.FirstOrDefault(i => i.QQ == qq);
            if (info == null)
            {
                info = new DrawInfo() { QQ = qq, Crystal = 0, Draw = 0, DrawTen = 0 };
                db.DrawInfos.Add(info);
            }
            switch (type)
            {
                case DrawInfoType.Crystal:
                    info.Crystal = count;
                    break;
                case DrawInfoType.Draw:
                    info.Draw = count;
                    break;
                case DrawInfoType.DrawTen:
                    info.DrawTen = count;
                    break;
                default:
                    return false;
            }
            db.SaveChanges();
            return true;
        }
    }

}
