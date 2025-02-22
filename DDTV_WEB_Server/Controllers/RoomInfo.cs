using DDTV_Core.SystemAssembly.BilibiliModule.Rooms;
using DDTV_Core.SystemAssembly.ConfigModule;
using DDTV_Core.SystemAssembly.DownloadModule;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DDTV_WEB_Server.Controllers
{
    public class Room_AllInfo : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "Room_AllInfo")]
        public ActionResult Post([FromForm] string cmd)
        {
            //Response.ContentType = "application/json";
            
            List<RoomInfoClass.RoomInfo> roomInfos = new();
            foreach (var item in Rooms.RoomInfo)
            {
                roomInfos.Add(new RoomInfoClass.RoomInfo { 
                    area=item.Value.area,
                    area_name=item.Value.area_name,
                    area_v2_id=item.Value.area_v2_id,
                    area_v2_name=item.Value.area_v2_name,
                    area_v2_parent_id=item.Value.area_v2_parent_id,
                    area_v2_parent_name=item.Value.area_v2_parent_name,
                    attention=item.Value.attention,
                    IsAutoRec=item.Value.IsAutoRec,
                    broadcast_type=item.Value.broadcast_type,
                    cover_from_user=item.Value.cover_from_user,
                    DanmuFile=null,
                    description= null,
                    Description= null,
                    encrypted=item.Value.encrypted,
                    face=item.Value.face,
                    hidden_till=item.Value.hidden_till,
                    IsCliping=item.Value.IsCliping,
                    IsDownload=item.Value.IsDownload,
                    IsRecDanmu=item.Value.IsRecDanmu,
                    IsRemind=item.Value.IsRemind,
                    is_hidden=item.Value.is_hidden,
                    is_locked=item.Value.is_locked,
                    is_portrait=item.Value.is_portrait,
                    is_sp=item.Value.is_sp,
                    keyframe=item.Value.keyframe,
                    level=item.Value.level,
                    Like=item.Value.Like,
                    live_status=item.Value.live_status,
                    live_time=item.Value.live_time,
                    lock_till=item.Value.lock_till,
                    need_p2p=item.Value.need_p2p,
                    online=item.Value.online,
                    pwd_verified=item.Value.pwd_verified,
                    roomStatus=item.Value.roomStatus,
                    room_id=item.Value.room_id,
                    room_shield=item.Value.room_shield,
                    roundStatus=item.Value.roundStatus,
                    sex=item.Value.sex,
                    short_id=item.Value.short_id,
                    sign=item.Value.sign,
                    special_type=item.Value.special_type,
                    tags=item.Value.tags,
                    tag_name=item.Value.tag_name,
                    title=item.Value.title,
                    uid=item.Value.uid,
                    uname=item.Value.uname,
                    url=item.Value.url,
                    roomWebSocket=null,
                    DownloadedLog=null,
                    DownloadingList=null,
                });
            }

            return Content(MessageBase.Success(nameof(Room_AllInfo), roomInfos), "application/json");
        }
    }

    public class Room_SummaryInfo : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "Room_SummaryInfo")]
        public ActionResult Post([FromForm] string cmd)
        {
            //Response.ContentType = "application/json";

            List<RoomInfoClass.RoomInfo> roomInfos = new();
            foreach (var item in Rooms.RoomInfo)
            {
                roomInfos.Add(new RoomInfoClass.RoomInfo
                {
                    IsAutoRec = item.Value.IsAutoRec,
                    IsDownload = item.Value.IsDownload,
                    IsRecDanmu = item.Value.IsRecDanmu,
                    IsRemind = item.Value.IsRemind,           
                    Like = item.Value.Like,
                    room_id = item.Value.room_id,
                    uid = item.Value.uid,
                    uname = item.Value.uname,
                });
            }

            return Content(MessageBase.Success(nameof(Room_AllInfo), roomInfos), "application/json");
        }
    }

    public class Room_Add : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "Room_Add")]
        public ActionResult Post([FromForm] long uid, [FromForm] string cmd)
        {
            if (int.TryParse(Rooms.GetValue(uid, DDTV_Core.SystemAssembly.DataCacheModule.DataCacheClass.CacheType.room_id), out int RoomId))
            {
                RoomConfig.AddRoom(uid, RoomId, "", true);
                return Content(MessageBase.Success(nameof(Room_Add), "添加完成"), "application/json");
            }
            else
            {
                return Content(MessageBase.Success(nameof(Room_Add), "该UID不存在！", "该UID不存在！",MessageBase.code.UIDFailed), "application/json");
            }
        }
    }
    public class Room_Add_ForRoomId : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "Room_Add_ForRoomId")]
        public ActionResult Post([FromForm] int roomid, [FromForm] string cmd)
        {
            long UID = 0;
            var roomInfo = DDTV_Core.SystemAssembly.BilibiliModule.API.RoomInfo.get_info(0, roomid, false);
            if (roomInfo != null)
            {
                UID = roomInfo.uid;
                RoomConfig.AddRoom(UID, roomid, "", true);
                return Content(MessageBase.Success(nameof(Room_Add), "添加完成"), "application/json");
            }
            else
            {
                return Content(MessageBase.Success(nameof(Room_Add), "该房间号不存在！", "该房间号不存在！", MessageBase.code.UIDFailed), "application/json");
            }
        }
    }
    public class Room_Del : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "Room_Del")]
        public ActionResult Post([FromForm] long uid, [FromForm] string cmd)
        {
            if(RoomConfig.DeleteRoom(uid))
            {
                return Content(MessageBase.Success(nameof(Room_Del), "删除完成"), "application/json");
            }
            else
            {
                return Content(MessageBase.Success(nameof(Room_Del), "该房间不存在或出现未知错误，删除失败", "该房间不存在或出现未知错误，删除失败",MessageBase.code.APIAuthenticationFailed), "application/json");
            }         
        }
    }
    public class Room_AutoRec : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "Room_AutoRec")]
        public ActionResult Post([FromForm] long uid, [FromForm] bool IsAutoRec, [FromForm] string cmd)
        {
            RoomConfigClass.RoomCard roomCard = new RoomConfigClass.RoomCard()
            {
                UID = uid,
                IsAutoRec = IsAutoRec
            };
            if (RoomConfig.ReviseRoom(roomCard, false, 2))
            {
               
                if (IsAutoRec && Rooms.GetValue(uid, DDTV_Core.SystemAssembly.DataCacheModule.DataCacheClass.CacheType.live_status) == "1")
                {
                    Download.AddDownloadTaskd(uid, true);
                }
                return Content(MessageBase.Success(nameof(Room_AutoRec), "已" + (IsAutoRec ? "打开" : "关闭") + $"UID为{uid}的房间开播自动录制"), "application/json");
            }
            else
            {
                return Content(MessageBase.Success(nameof(Room_AutoRec), $"修改UID为{uid}的开播自动录制出现问题，修改失败", $"修改UID为{uid}的开播自动录制出现问题，修改失败",MessageBase.code.OperationFailed), "application/json");
            }
        }
    }
    public class Room_DanmuRec : ProcessingControllerBase.ApiControllerBase
    {
        [HttpPost(Name = "Room_DanmuRec")]
        public ActionResult Post([FromForm] long uid, [FromForm] bool IsRecDanmu, [FromForm] string cmd)
        {
            RoomConfigClass.RoomCard roomCard = new RoomConfigClass.RoomCard()
            {
                UID = uid,
                IsRecDanmu = IsRecDanmu
            };
            if (RoomConfig.ReviseRoom(roomCard, false, 6))
            {
                return Content(MessageBase.Success(nameof(Room_DanmuRec), "已" + (IsRecDanmu ? "打开" : "关闭") + $"UID为{uid}的弹幕录制"), "application/json");
            }
            else
            {
                return Content(MessageBase.Success(nameof(Room_DanmuRec), $"修改UID为{uid}的弹幕录制出现问题，修改失败", $"修改UID为{uid}的弹幕录制出现问题，修改失败", MessageBase.code.OperationFailed), "application/json");
            }
        }
    }
}