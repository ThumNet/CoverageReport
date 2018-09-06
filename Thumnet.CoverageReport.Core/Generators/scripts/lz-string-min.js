﻿// Copyright (c) 2013 Pieroxy <pieroxy@pieroxy.net>
// This work is free. You can redistribute it and/or modify it
// under the terms of the WTFPL, Version 2
// For more information see LICENSE.txt or http://www.wtfpl.net/
//
// For more information, the home page:
// http://pieroxy.net/blog/pages/lz-string/testing.html
//
// LZ-based compression algorithm, version 1.4.4
var LZString144=function(){var w=String.fromCharCode,n="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",e="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$",t={};function i(o,r){if(!t[o]){t[o]={};for(var n=0;n<o.length;n++)t[o][o.charAt(n)]=n}return t[o][r]}var s={compressToBase64:function(o){if(null==o)return"";var r=s._compress(o,6,function(o){return n.charAt(o)});switch(r.length%4){default:case 0:return r;case 1:return r+"===";case 2:return r+"==";case 3:return r+"="}},decompressFromBase64:function(r){return null==r?"":""==r?null:s._decompress(r.length,32,function(o){return i(n,r.charAt(o))})},compressToUTF16:function(o){return null==o?"":s._compress(o,15,function(o){return w(o+32)})+" "},decompressFromUTF16:function(r){return null==r?"":""==r?null:s._decompress(r.length,16384,function(o){return r.charCodeAt(o)-32})},compressToUint8Array:function(o){for(var r=s.compress(o),n=new Uint8Array(2*r.length),e=0,t=r.length;e<t;e++){var i=r.charCodeAt(e);n[2*e]=i>>>8,n[2*e+1]=i%256}return n},decompressFromUint8Array:function(o){if(null==o)return s.decompress(o);for(var r=new Array(o.length/2),n=0,e=r.length;n<e;n++)r[n]=256*o[2*n]+o[2*n+1];var t=[];return r.forEach(function(o){t.push(w(o))}),s.decompress(t.join(""))},compressToEncodedURIComponent:function(o){return null==o?"":s._compress(o,6,function(o){return e.charAt(o)})},decompressFromEncodedURIComponent:function(r){return null==r?"":""==r?null:(r=r.replace(/ /g,"+"),s._decompress(r.length,32,function(o){return i(e,r.charAt(o))}))},compress:function(o){return s._compress(o,16,function(o){return w(o)})},_compress:function(o,r,n){if(null==o)return"";var e,t,i,s={},p={},u="",c="",a="",l=2,f=3,h=2,d=[],m=0,v=0;for(i=0;i<o.length;i+=1)if(u=o.charAt(i),Object.prototype.hasOwnProperty.call(s,u)||(s[u]=f++,p[u]=!0),c=a+u,Object.prototype.hasOwnProperty.call(s,c))a=c;else{if(Object.prototype.hasOwnProperty.call(p,a)){if(a.charCodeAt(0)<256){for(e=0;e<h;e++)m<<=1,v==r-1?(v=0,d.push(n(m)),m=0):v++;for(t=a.charCodeAt(0),e=0;e<8;e++)m=m<<1|1&t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t>>=1}else{for(t=1,e=0;e<h;e++)m=m<<1|t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t=0;for(t=a.charCodeAt(0),e=0;e<16;e++)m=m<<1|1&t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t>>=1}0==--l&&(l=Math.pow(2,h),h++),delete p[a]}else for(t=s[a],e=0;e<h;e++)m=m<<1|1&t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t>>=1;0==--l&&(l=Math.pow(2,h),h++),s[c]=f++,a=String(u)}if(""!==a){if(Object.prototype.hasOwnProperty.call(p,a)){if(a.charCodeAt(0)<256){for(e=0;e<h;e++)m<<=1,v==r-1?(v=0,d.push(n(m)),m=0):v++;for(t=a.charCodeAt(0),e=0;e<8;e++)m=m<<1|1&t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t>>=1}else{for(t=1,e=0;e<h;e++)m=m<<1|t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t=0;for(t=a.charCodeAt(0),e=0;e<16;e++)m=m<<1|1&t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t>>=1}0==--l&&(l=Math.pow(2,h),h++),delete p[a]}else for(t=s[a],e=0;e<h;e++)m=m<<1|1&t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t>>=1;0==--l&&(l=Math.pow(2,h),h++)}for(t=2,e=0;e<h;e++)m=m<<1|1&t,v==r-1?(v=0,d.push(n(m)),m=0):v++,t>>=1;for(;;){if(m<<=1,v==r-1){d.push(n(m));break}v++}return d.join("")},decompress:function(r){return null==r?"":""==r?null:s._decompress(r.length,32768,function(o){return r.charCodeAt(o)})},_decompress:function(o,r,n){var e,t,i,s,p,u,c,a=[],l=4,f=4,h=3,d="",m=[],v={val:n(0),position:r,index:1};for(e=0;e<3;e+=1)a[e]=e;for(i=0,p=Math.pow(2,2),u=1;u!=p;)s=v.val&v.position,v.position>>=1,0==v.position&&(v.position=r,v.val=n(v.index++)),i|=(0<s?1:0)*u,u<<=1;switch(i){case 0:for(i=0,p=Math.pow(2,8),u=1;u!=p;)s=v.val&v.position,v.position>>=1,0==v.position&&(v.position=r,v.val=n(v.index++)),i|=(0<s?1:0)*u,u<<=1;c=w(i);break;case 1:for(i=0,p=Math.pow(2,16),u=1;u!=p;)s=v.val&v.position,v.position>>=1,0==v.position&&(v.position=r,v.val=n(v.index++)),i|=(0<s?1:0)*u,u<<=1;c=w(i);break;case 2:return""}for(t=a[3]=c,m.push(c);;){if(v.index>o)return"";for(i=0,p=Math.pow(2,h),u=1;u!=p;)s=v.val&v.position,v.position>>=1,0==v.position&&(v.position=r,v.val=n(v.index++)),i|=(0<s?1:0)*u,u<<=1;switch(c=i){case 0:for(i=0,p=Math.pow(2,8),u=1;u!=p;)s=v.val&v.position,v.position>>=1,0==v.position&&(v.position=r,v.val=n(v.index++)),i|=(0<s?1:0)*u,u<<=1;a[f++]=w(i),c=f-1,l--;break;case 1:for(i=0,p=Math.pow(2,16),u=1;u!=p;)s=v.val&v.position,v.position>>=1,0==v.position&&(v.position=r,v.val=n(v.index++)),i|=(0<s?1:0)*u,u<<=1;a[f++]=w(i),c=f-1,l--;break;case 2:return m.join("")}if(0==l&&(l=Math.pow(2,h),h++),a[c])d=a[c];else{if(c!==f)return null;d=t+t.charAt(0)}m.push(d),a[f++]=t+d.charAt(0),t=d,0==--l&&(l=Math.pow(2,h),h++)}}};return s}();"function"==typeof define&&define.amd?define(function(){return LZString144}):"undefined"!=typeof module&&null!=module&&(module.exports=LZString144);