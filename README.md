# web打印代理
## 简介
一款静默打印代理工具，通过建立Socket服务，接收打印指令，通过解析打印指令完成打印。
## 功能描述

### 系统功能
* 最小化到状态栏
* 单实例
* 保留异常日志
* 保留打印日志

### 打印功能
* 自定义纸张大小
* 手动分页
* 打印条形码，二维码
* 打印图片

## 指令示例
使用JSON格式
类型：text/qrcoe/barcode/line
web->代理
```
{
  "id":1537148688,  // 唯一id，使用当前时间戳
  "data":{
    "page":{
        "width":190,
        "height":80
      },
    "content":
    [
      {
        "type":"text",
        "value":"打印的内容",
        "size":20,
        "x":40,
        "y":5
      },
      {
        "type":"qrcode",
        "value":"打印的内容",
        "width":70,
        "height":70,
        "x":55,
        "y":15
      }
    ]
  }
}
```
代理->web
```
{
  id:1537148688,
  status:0,//0成功，1失败
  msg:"返回的消息"
}
```
打印内容
```
{
  page:[20,20],// 页面尺寸，高/宽
  data:
  [
    {
      type:"text",
      value:"打印的内容",
      position:[x,y],
      size:[a,b],// 字号，旋转角度
    },
    {
      type:"qrcode",
      value:"打印的内容",
      position:[x,y],
      size:[a,b],// 高/宽
    },
    {
      type:"barcode",
      value:"打印的内容",
      position:[x,y],
      size:[a,b],// 高/宽
    }
  ]
}
```

## 注意
有几个依赖来自于本人自己的NuGet服务，需要的请留言。