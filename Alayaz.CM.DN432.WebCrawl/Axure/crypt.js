//全局变量
var serverPacket="";  	//服务器回传数据包
var clientAuthCode="";	//客户端认证码
var serverRandom="";    //服务器回传的随机数
var operType="";      	//操作类型
var userId="";        	//用户ID
var userPasswd = "";// 用户口令
var deviceType = "";	//设备类型
var devicePort = "";	//设备端口号
var userName="";     //用户姓名
var clientPubKey=""; //客户端公钥
var sysName="";      //子系统名称
var sysCode="";      //子系统代码
var userAccount="";  //用户登录子系统的帐号
var userPin="";      //用户登录子系统的密码

var strContainer= "";//CTAS0001        "
var strProvider = "";//WatchData Cryptographic Provider v3.0"
var nProvType   = 1;//用于设备类型的开关选择


// 保存用户对设备的选择
function setDeviceParam(userPin)
{
    if (CryptCtrl.IsDeviceOpened()!= 0)
    {
    	CryptCtrl.CloseDevice();
    }
    CryptCtrl.strContainer = "";

    userPasswd = userPin;
}

//打开设备
function openDevice(userPin)
{	

   	var err = 0;
    
    setDeviceParam(userPin);
   
    if (CryptCtrl.IsDeviceOpened()!= 0)
    {
    	CryptCtrl.CloseDevice();
    }
    
   CryptCtrl.OpenDeviceEx(userPasswd) ;
    if(CryptCtrl.ErrCode==0x57)
    {
        CryptCtrl.OpenDeviceEx(userPasswd);
    }
    
    if (CryptCtrl.ErrCode != 0 && CryptCtrl.ErrCode != -1)
    {
    	jAlert("<div id='popup_message'>"+CryptCtrl.ErrMsg+"</div>","提示");
       return CryptCtrl.ErrCode;
    }

    devicePort=CryptCtrl.strContainer;
	   
    return CryptCtrl.ErrCode;
}

//产生客户端数据包ClientHello
function MakeClientHello()
{
    //只要不是纯粹单向认证,就需要服务器证书
    var vbNullString="";
    var dwFlag=0;
   	
   	CryptCtrl.ClientHello(dwFlag);
   	if(CryptCtrl.ErrCode != 0)
    {
    	jAlert("<div id='popup_message'>"+CryptCtrl.ErrMsg+"</div>","提示");
       return CryptCtrl.ErrCode;
    }

   	return CryptCtrl.ErrCode;
}

//验证服务器端的serverHello数据包，并生成客户端认证码
function MakeClientAuthCode()
{
  	var err = 0;
  	err = openDevice();

  	if (err != 0) return err;
    //客户端验证ServerHello

   	CryptCtrl.ClientAuth(serverPacket);

   	if (CryptCtrl.ErrCode != 0)
   	{
    	jAlert("<div id='popup_message'>"+CryptCtrl.ErrMsg+"</div>","提示");
       return CryptCtrl.ErrCode;
   	}
   	clientAuthCode = CryptCtrl.strResult;
    //产生客户端认证码后，关闭客户端
    CryptCtrl.CloseDevice();
    //userId = CryptCtrl.strUserId;
    return CryptCtrl.ErrCode;
}

//修改用户口令
function changePin()
{
   if(openDevice()==0)
   {
        CryptCtrl.ChangePinEx();
        jAlert("<div id='popup_message'>"+CryptCtrl.ErrMsg+"</div>","提示");
       return CryptCtrl.ErrCode;
   }
}

//对数据进行数字签名
function signData(strData,signTime,flag)
{
	var err = 0;
	//打开客户端设备，在使用客户端设备前必须执行打开设备动作
	err = openDevice();
	if (err != 0) return err;
	//数字签名函数
	CryptCtrl.SignData(strData,strData.length,"SHA1withRSA",signTime,flag);
	if(CryptCtrl.ErrCode != 0)
	{
	     jAlert("<div id='popup_message'>"+CryptCtrl.ErrMsg+"</div>","提示");
       return CryptCtrl.ErrCode;
	}

	//签名结果
	strSignedCode = CryptCtrl.strResult;
	//关闭设备函数
	CryptCtrl.CloseDevice();
	return CryptCtrl.ErrCode;
}

function getCert(){

  var rtn = openDevice();
  var ret = CryptCtrl.GetCertInfo("",71);
  var error = CryptCtrl.errCode;
  var nsrsbh="";
  if(error==0){
      nsrsbh = CryptCtrl.strResult;
  }
	CryptCtrl.CloseDevice();
  return nsrsbh;
}

