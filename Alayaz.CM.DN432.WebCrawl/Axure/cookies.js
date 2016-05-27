var Ip="";

var seconds=10*60;//10分钟

function getCookie(name){
  var nameEQ=name+"=";
  var str=document.cookie.split(';');
  for(var i=0;i<str.length;i++){
    var c=str[i];
    while(c.charAt(0)==' '){
      c=c.substring(1,c.length);
    }
    if(c.indexOf(nameEQ)==0){
      return unescape(c.substring(nameEQ.length,c.length));
    }
  }
  return "";
}
    
function clearCookie(name) {    
  setCookie(name, "", -1);    
}    
        
function setCookie(name, value, seconds) {    
  seconds = seconds || 0;       
  var expires = "";    
  if (seconds != 0 ) {        
   var date = new Date();    
   date.setTime(date.getTime()+(seconds*1000));    
   expires = "; expires="+date.toGMTString();    
  }    
  document.cookie = name+"="+escape(value)+expires+"; path=/";       
}