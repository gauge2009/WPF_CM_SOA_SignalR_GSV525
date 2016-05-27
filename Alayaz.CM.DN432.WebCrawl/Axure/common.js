


function isIEWeb(){
    if (window.ActiveXObject ||"ActiveXObject" in window) {
        return true;
    }else{
        return false;
    }
}

//yyyy-mm转换成yyyy年mm月
function getDqSsq(dqrq){
	var year=dqrq.substring(0,4);
  var month=parseInt(dqrq.substring(5,7),10);

  return year+"年"+month+"月";
}

//获取当前时间的YYYY-MM格式
function getYYYYMM(){
  var myDate = new Date();
  var year=myDate.getFullYear();
  var month=myDate.getMonth()+1;
  if(month<10){
    month="0"+month;
  }

  return year+'-'+month;
}

//获取以客户端时间为基准的认证月份
function getRzyf(){
  var myDate = new Date();
  var year=myDate.getFullYear();
  var month=myDate.getMonth()+1;
  if(month==1){
    year-=1;
    month=12;
  }else{
    month-=1;
  }
  if(month<10){
    month="0"+month;
  }

  return year+month;
}

//获取以客户端时间为基准的认证月份
function getXtRzyf(dqrq){
  var year=parseInt(dqrq.substring(0,4),10);
  var month=parseInt(dqrq.substring(5),10);
  if(month==1){
    year-=1;
    month=12;
  }else{
    month-=1;
  }
  if(month<10){
    month="0"+month;
  }

  return year+month;
}

function getStartRzYf(dqrq){
    var year=parseInt(dqrq.substring(0,4),10);
    var month=parseInt(dqrq.substring(5),10);
    if(month==1){
        year-=1;
        month=11;
    }else if(month==2){
      year-=1;
      month=12;
    }else{
      month-=2;
    }
    if(month<10){
      month="0"+month;
    }
    return year+month;
}


//获取当前时间yyyy-mm-dd
function getDqrq(){
  var myDate = new Date();
  var year=myDate.getFullYear();
  var month=myDate.getMonth()+1;
  var day=myDate.getDate();
  if(month<10){
    month="0"+month;
  }
  if(day<10){
    day="0"+day;
  }

  return year+"-"+month+"-"+day;
}

function getRzcxDate(dqrq){
    var year=parseInt(dqrq.substring(0,4),10);
    var month=parseInt(dqrq.substring(5,7),10);
    var day=parseInt(dqrq.substring(8),10);
    if(month==1){
        year-=1;
        month=11;
    }else if(month==2){
      year-=1;
      month=12;
    }else{
      month-=2;
    }
    if(month<10){
      month="0"+month;
    }
    return year+"-"+month+"-"+"01";
}

function getBefore3YYYYMM(){
  var myDate = new Date();
  var year=myDate.getFullYear();
  var month=myDate.getMonth()+1;
  if(month<3){
    year=year-1;
    month=12+month-2;
  }else{
    month=month-2;
  }
  if(month<10){
    month="0"+month;
  }

  return year+'-'+month;
}

//根据客户端时间判断
function getNowDate(index){
    var myDate = new Date();
    var year=myDate.getFullYear();
    var month=myDate.getMonth()+1;
    var day=myDate.getDate(); 

    if(index==1){
        if(month==1){
             year-=1;
             month=12;
        }else{
            month-=1;
        }
        day=1;
    }else if(index==2){
      if(month==1){
        month=12;
        year-=1;
      }else{
        month-=1;
      }
      day=getDayNum(year,month);
    }        
    if(month<10){
        month="0"+month;
    }  
    if(day<10){
      day="0"+day;
    }   
    return year+"-"+month+"-"+day;
}

//根据服务器时间判断
function getXtsj(dqrq,index){

    var year=parseInt(dqrq.substring(0,4),10);
    var month=parseInt(dqrq.substring(5,7),10);
    var day=parseInt(dqrq.substring(8),10);

    if(index==1){
        if(month==1){
             year-=1;
             month=12;
        }else{
            month-=1;
        }
        day=1;
    }else if(index==2){
      if(month==1){
        month=12;
        year-=1;
      }else{
        month-=1;
      }
      day=getDayNum(year,month);
    }          
    if(month<10){
        month="0"+month;
    }  
    if(day<10){
      day="0"+day;
    }   
    return year+"-"+month+"-"+day;
}

function getDayNum(year,month){
  var c=0;
  if(month==2){   
    if (((year % 4)==0) && ((year % 100)!=0) || ((year % 400)==0)) {
      c=29;
    }else{
      c=28;
    }
  }else if(month==4||month==6||month==9||month==11){
    c=30;
  }else{
    c=31;
  }
  return c;
}

function getFormatDate(value){
    var date=value.substring(0,10);
    var hour=value.substring(11);
    var arr1=date.split('-');
    var arr2=hour.split(':');

    return (arr1[0]+"年"+arr1[1]+"月"+arr1[2]+"日"+arr2[0]+"时"+arr2[1]+"分"+arr2[2]+"秒");

}

function getFormatYuan(value){
  value=value+'';
  var result='';
  var restr='';
  if(value!=""){
    var arr=value.split('.');
    var length1=arr[0].length;
    var dotCount=parseInt(length1/3);
    var arr1=arr[0].split('');
    for(var i=length1-1;i>=0;){
      result+=arr1[i];
      if(i-1>=0){
        result+=arr1[i-1];
      }
      if(i-2>=0){
        result+=arr1[i-2];
      }
      if(i-3>=0){
        result+=',';
      }
      i=i-3;
    } 
    restr=result.split("").reverse().join("");
    if(arr.length==2){
      restr+='.'+arr[1];
    } else if(arr.length==1){
      restr+='.00';
    }
  }
  return restr;
}

function getFpfs(value){
  var result=0;
  if(value!=""){
    result=value;
    var index=value.indexOf(".");
    if(index>0){
      result=value.substring(0,index);
    }
  }
  return result;
}

function getHxResult(value){
  var result="";
  if(value=="1"){
    result="勾选";
  }else if(value=="0"){
    result="撤销";
  }
  return result;
}

function dropDot(value){
  var index=value.indexOf('.');
  if(index>=0){
    value=value.substring(0,index);
  }
  return value;
}

function getWordDate(value){
  var date="";
  if(value.length==6){
    date=value.substring(0,4)+"年"+value.substring(4,6)+"月";
  }else{
    date="";
  }
  return date;
}

function jungeIE(){
    var browser=navigator.appName 
    var b_version=navigator.appVersion 
    var version=b_version.split(";"); 
    var trim_Version=version[1].replace(/[ ]/g,""); 
    var result="Y";
    if(browser=="Microsoft Internet Explorer" && trim_Version=="MSIE6.0") 
    { 
      result="N";
    } 
    else if(browser=="Microsoft Internet Explorer" && trim_Version=="MSIE7.0") 
    { 
      result="N"; 
    } 
    else if(browser=="Microsoft Internet Explorer" && trim_Version=="MSIE8.0") 
    { 
      result="N";
    } 
    else if(browser=="Microsoft Internet Explorer" && trim_Version=="MSIE9.0") 
    { 
      result="N";
    } 
    return result;
}

function quit(){
    jConfirm("<div id='popup_message'>确定退出系统？</div>", '确认信息', function(r) {
      if(r){
    		var cert=getCert();
        var strRegx=/^[0-9a-zA-Z]+$/;

    		if(cert==""||!strRegx.test(cert)){
    			window.location="index.html";
          clearCookie("token");  
          clearCookie("nsrmc"); 
          clearCookie("dqrq"); 
    			return;
    		}
        $.ajax({
                type:"post",
                url:Ip+"/SbsqWW/quit.do",
                data:{"cert":cert},
                dataType: "jsonp",
                callback:"json",
                success: function(jsonData){
                     var key1=jsonData.key1; 
                     clearCookie("token");  
                     clearCookie("nsrmc");  
                     clearCookie("dqrq");   
                     if(key1=="01"){
                        window.location="index.html";
                     }else if(key1=="03"){
                        jAlert("<div id='popup_message'>系统异常</div>！","提示");
                        return;
                     }
                },
                error:function(err){
                    window.location="index.html";
                }
           });
      }
    });
}


// JavaScript Document

/*第一种形式 第二种形式 更换显示样式*/
function setTab(name,cursel,n){
for(i=1;i<=n;i++){
var menu=document.getElementById(name+i);
var con=document.getElementById("con_"+name+"_"+i);
menu.className=i==cursel?"hover":"";
con.style.display=i==cursel?"block":"none";
}
}



/*不规则tab*/
function setTab03Syn ( i )
  {
    selectTab03Syn(i);
  }
  
  function selectTab03Syn ( i )
  {
    switch(i){
      case 1:
      document.getElementById("TabCon1").style.display="block";
      document.getElementById("TabCon2").style.display="none";
      document.getElementById("font1").style.color="#ffffff";
      document.getElementById("font2").style.color="#444444";
      break;
      case 2:
      document.getElementById("TabCon1").style.display="none";
      document.getElementById("TabCon2").style.display="block";
      document.getElementById("font1").style.color="#444444";
      document.getElementById("font2").style.color="#ffffff";
      break;
    }
  }



