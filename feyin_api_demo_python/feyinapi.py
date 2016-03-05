#!/usr/bin/env python
# encoding=utf-8
#
# Feyin API client code in Python
# Feyin API doc: http://www.feyin.net/doc
# 

import httplib,urllib
import hashlib
from datetime import datetime
import time

#商户会员的基本信息定义，这些信息可以从飞印平台的技术支持人员处获取
# Feyin API service account settings
MEMBER_CODE = '1eb79ba2a55f11e0be3384aaaa41add1'
FEYIN_KEY = 'b0bbe5c9'
FEYIN_HOST = 'my.feyin.net'
FEYIN_PORT = 80
FEYIN_KEEP_ALIVE = False

#提交自由格式的内容，推荐使用
# Submit free formatted content to print, recommended
def sendFreeMessage(msgInfo):
	reqTime = long(1000*time.time())
	md5 = hashlib.md5()
	content = '%s%s%s%s%s%s'%(msgInfo['memberCode'],msgInfo['msgDetail'],msgInfo['deviceNo'],msgInfo['msgNo'],reqTime,FEYIN_KEY)
	md5.update(content)
	msgInfo['reqTime'] = reqTime
	msgInfo['securityCode'] = md5.hexdigest()
	msgInfo['mode'] = 2
	sendContent = urllib.urlencode(msgInfo)

	return sendPostMessage(sendContent)
    
    
# 提交格式化的内容，不常使用
# Submit formatted content to print, not used very often
def sendFormatedMessage (msgInfo):
	reqTime = long(1000*time.time())
	md5 = hashlib.md5()
	content = '%s%s%s%s%s%s%s%s%s%s'%(msgInfo['memberCode'],msgInfo['customerName'],msgInfo['customerPhone'],msgInfo['customerAddress'],msgInfo['customerMemo'],msgInfo['msgDetail'],msgInfo['deviceNo'],msgInfo['msgNo'],reqTime,FEYIN_KEY)
	md5.update(content)
	msgInfo['reqTime'] = reqTime
	msgInfo['securityCode'] = md5.hexdigest()
	msgInfo['mode'] = 1
	sendContent = urllib.urlencode(msgInfo)	
	
	return sendPostMessage(sendContent)

    
# 公用的Post提交方法
# Common HTTP POST method definition
def sendPostMessage (sendContent):
	#定义文件头  
    #Define HTTP header
	headers = {"Content-Type":"application/x-www-form-urlencoded"}
	if FEYIN_KEEP_ALIVE:
		headers["Connection"] = "Keep-Alive"	
	conn = httplib.HTTPConnection(FEYIN_HOST,FEYIN_PORT)
	#开始进行数据提交
    #Start data submission
	conn.request(method="POST",url="/api/sendMsg",body=sendContent,headers=headers)
	#返回处理后的数据
    #Get HTTP response from API service
	response = conn.getresponse()
	#判断是否提交成功
    #Check if submission success or not
	if response.status == 302 or response.status == 200:
		msg = response.read()  
	else:  
		msg = "fail"
		print response.read()
	conn.close()
	return msg

    
# 公用的Get提交方法
# Common HTTP GET method definition
def sendGetMessage (url):
	#定义文件头  
	#headers = {"Content-Type":"application/x-www-form-urlencoded"}
	headers = {}
	if FEYIN_KEEP_ALIVE:
		headers["Connection"] = "Keep-Alive"	
	conn = httplib.HTTPConnection(FEYIN_HOST,FEYIN_PORT)
	#开始进行数据提交
    #Start data submission
	conn.request(method="GET",url=url)
	#返回处理后的数据  
    #Get HTTP response from API service
	response = conn.getresponse()
	#判断是否提交成功  
    #Check if submission is success or not
	if response.status == 302 or response.status == 200:
		msg = response.read()  
	else:  
		msg = "fail"
		print response.read()
	conn.close()
	return msg
	
    
# 查询打印状态
# Query print job status
def queryState (msgNo):
	reqTime = long(1000*time.time())
	md5 = hashlib.md5()
	content = '%s%s%s%s'%(MEMBER_CODE,reqTime,FEYIN_KEY,msgNo)
	md5.update(content)

	params = {'memberCode':MEMBER_CODE,'securityCode':md5.hexdigest(),'reqTime':reqTime,'msgNo':msgNo}
	url = "/api/queryState?memberCode=%s&securityCode=%s&reqTime=%d&msgNo=%s"%(MEMBER_CODE,params['securityCode'],reqTime,msgNo)

	return sendGetMessage(url)

    
# 查询设备列表
# Get printer list
def listDevice ():
	reqTime = long(1000*time.time())
	md5 = hashlib.md5()
	content = '%s%s%s'%(MEMBER_CODE,reqTime,FEYIN_KEY)
	md5.update(content)

	params = {'memberCode':MEMBER_CODE,'securityCode':md5.hexdigest(),'reqTime':reqTime}
	url = "/api/listDevice?memberCode=%s&securityCode=%s&reqTime=%d"%(MEMBER_CODE,params['securityCode'],reqTime)
	
	return sendGetMessage(url)
    

# 查询异常打印列表
# Query print job exception list
def listException ():
	reqTime = long(1000*time.time())
	md5 = hashlib.md5()
	content = '%s%s%s'%(MEMBER_CODE,reqTime,FEYIN_KEY)
	md5.update(content)

	params = {'memberCode':MEMBER_CODE,'securityCode':md5.hexdigest(),'reqTime':reqTime}
	sendContent = urllib.urlencode(params)

	url="/api/listException?memberCode=%s&securityCode=%s&reqTime=%d"%(MEMBER_CODE,params['securityCode'],reqTime)

	
	return sendGetMessage(url)
	
    
#--------------- Feyin API Print Test Functions ------------------
# 测试发送格式化的消息
# Test print formatted information
def testSendFormatedMsg ():
	msgNo = datetime.now().strftime('%Y%m%d%I%M%S')+MEMBER_CODE
	# 定义消息的详细数据，注意，凡是有中文的地方，需要转成utf8编码
    # 以下所用的格式化输出模板字段必须先在飞印中心(http://my.feyin.net)定义
    # Define print job detail. Use utf8 encoding for any non-ASCII text.
    # Data field template below must be pre-defined in your Feyin API service (http://my.feyin.net)
	msgInfo = {
		'memberCode':MEMBER_CODE,  
		'charge':'3000',  
		'customerName':u'刘小姐'.encode('utf-8'),  
		'customerPhone':'13321332245',  
		'customerAddress':u'五山华南理工'.encode('utf-8'),  
		'customerMemo':u'请快点送货'.encode('utf-8'),  
		'msgDetail':u'番茄炒粉@1000@1||客家咸香鸡@2000@1'.encode('utf-8'),  
		'deviceNo':'4600275362700000',  
		'msgNo':msgNo
	}

	returnVal = sendFormatedMessage(msgInfo)

	print u'API response: %s' % returnVal
	print u'Print Job ID: %s'% msgNo

# 测试发送自由格式的消息
# Test free form printing
def testSendFreeMessage():
	msgNo = datetime.now().strftime('%Y%m%d%I%M%S')+'0'+MEMBER_CODE
	#定义消息的详细数据，注意，凡是有中文的地方，需要转成utf8编码
	freeMsgInfo = {
		'memberCode':MEMBER_CODE,  
		'msgDetail': u'\n “飞印是短消息云打印服务，通过移动网络把您需要的网络信息发送打印到世界的任何角落。” \n \n "Feyin is a short message cloud printing solution that enables remote printing of Internet info to anywhere in the world." \n'.encode('utf-8'),  
		'deviceNo':'4600275362700000',  
		'msgNo':msgNo
	}

	returnVal = sendFreeMessage(freeMsgInfo)

	print u'API response code: %s' % returnVal
	print u'Print Job ID: %s'% msgNo

# 测试获取设备列表
# Test get printer list
def testListDevice ():
	printerList = listDevice()
	print u'Your printer list:'
	print printerList
	
# 测试获取异常消息列表
# Test get print job exception list
def testListException ():
	printJobException = listException()
	print u'Pront job exception response:'
	print printJobException


# 测试查询打印任务状态
# Test get printer list
def testQueryState (msgNo):
	printerStatus = queryState(msgNo)
	print u'Printer status code: %s' % printerStatus

if __name__ == '__main__':

	#测试发送格式化的打印消息
    #Test print formatted messages
	#testSendFormatedMsg()

	#测试发送自由格式的打印消息（推荐方式，最简单）
    #Test print free form messages (recommended, simplest)
	testSendFreeMessage()

	#测试查询设备列表
    #Test query device list
	#testListDevice()

	#测试查询异常消息列表
    #Test query print job exception
	#testListException()

	#测试查询打印任务状态
    #Test query print job status	

    #从执行上述测试打印函数testSendFreeMessage() 或 testSendFormatedMsg() 过程获得服务端
    #反馈的打印请求ID，分配给下面的printJobID即可手动查询具体打印任务的执行状态
    #Get a Print Job ID from testSendFreeMessage() or testSendFormatedMsg() above
    #Assign it to msgNo below, then you can query it's print job status
    #time.sleep(2)
    #printJobID = "2012061902513401eb79ba2a55f11e0be33842bbbbbadd1"
    #testQueryState(printJobID)
