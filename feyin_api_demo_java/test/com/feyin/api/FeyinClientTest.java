package com.feyin.api;

/**
 * 飞印api测试
 * 
 * @author tommy.zeng
 * 
 */
public class FeyinClientTest {
	
	//使用本测试代码，您需要设置以下3项变量
	//@ MEMBER_CODE：商户代码，登录飞印后在“API集成”->“获取API集成信息”获取
	//@ FEYIN_KEY：密钥，获取方法同上
	//@ DEVICE_NO：打印机设备编码，通过打印机后面的激活按键获取，为16位数字，例如String deviceNo = "4600365507768327";
	public static final String MEMBER_CODE = "1eb79ba2a55f11e0be33842b2b41add1";
	public static final String FEYIN_KEY = "b06ae5c9";
	public static final String DEVICE_NO = "4600365507765058";
	
	//以下2项是平台相关的设置，您不需要更改
	public static final String FEYIN_HOST = "my.feyin.net";
	public static final int FEYIN_PORT = 80;

	/**
	 * @param args
	 */
	public static void main(String[] args) throws Exception {
		FeyinClient client = new FeyinClient(FEYIN_HOST, FEYIN_PORT, FEYIN_KEY,
				MEMBER_CODE);
		
		String msgNo;

		//---------------------测试发送格式化的打印消息-----------------
		msgNo = testSendFormatedMessage(client);
		
		//---------------------测试发送自由格式的打印消息-----------------
		//msgNo = testSendFreeMessage(client);
		
		//---------------------测试查询打印状态----------------------------
		//0：发送中                                                    1：已打印                                        2：打印失败
		//9：发送成功，但未打印                    -1：IP不允许                                -2：关键参数为空或请求方式不对
		//-3：客户编码不对                                -4：安全校验码不正确             -5：请求时间失效
		//-6：订单编号不对
		//testQueryState(client,msgNo);
		
		//---------------------测试查询设备列表----------------------------
		//testListDevice(client);
		
		//---------------------测试查询异常消息列表----------------------------
		//testListException(client);
	}

	private static void testListException(FeyinClient client) {
		String content = "";

		try {
			content = client.listException();
		} catch (Exception e) {
			e.printStackTrace();
		}

		System.out.println(content);
	}
	
	
	private static void testListDevice(FeyinClient client) {

		String content = "";

		try {
			content = client.listDevice();
		} catch (Exception e) {
			e.printStackTrace();
		}

		System.out.println(content);
	}

	private static String testSendFreeMessage(FeyinClient client) {
		
		String memberCode = MEMBER_CODE;
		String deviceNo = DEVICE_NO;
		
		String msgNo = String.valueOf(System.currentTimeMillis());
		int returnVal = -10;
		
		String msgDetail;
		msgDetail  = "    北京订餐网欢迎您订购\r\n";
		msgDetail += "\r\n";
		msgDetail += "\r\n";
		msgDetail += "条目         单价（元）    数量\r\n";
		msgDetail += "------------------------------\r\n";
		msgDetail += "番茄炒粉\r\n";
		msgDetail += "             10.0          1\r\n";
		msgDetail += "客家咸香鸡\r\n";
		msgDetail += "             20.0          1\r\n";
		msgDetail += "\r\n";
		msgDetail += "备注：快点送到。\r\n";
		msgDetail += "------------------------------\r\n";
		msgDetail += "合计：30.0元 \r\n";
		msgDetail += "\r\n";
		msgDetail += "送货地址：北京中关村科技大厦23楼\r\n";
		msgDetail += "联系电话：1380017****\r\n";
		msgDetail += "订购时间：2011-01-06 19:30:10\r\n";

		// 发送自由格式的消息
		//以下参数是必须的，没有设置将导致发送出错
		FeyinMessage freeMessage = new FeyinMessage(
				FeyinMessageType.freeMessage, memberCode, deviceNo, msgDetail);
		
		//以下参数并不是必须的，您可以根据需要设置
		freeMessage.setMsgNo(msgNo);		//不设置将由飞印平台自动生成
		
		try {
			returnVal = client.sendFreeMessage(freeMessage);
		} catch (Exception e) {
			returnVal = -10;
			System.err.println(e);
		}
		dealWithReturnValue(returnVal);
		
		return msgNo;
	}

	private static String testSendFormatedMessage(FeyinClient client) {
		String memberCode = MEMBER_CODE;
		String deviceNo = DEVICE_NO;
		
		String charge = "3000";
		String customerName = "刘小姐";
		String customerPhone = "13321332240";
		String customerAddress = "五山华南理工";
		String customerMemo = "请快点送货";
		String msgDetail = "番茄炒粉@1000@1||客家咸香鸡@2000@1";
		String extra1 = "扩展内容1";
		String msgNo = String.valueOf(System.currentTimeMillis());

		// 发送格式化的信息（如订单）
		//以下参数是必须的，没有设置将导致发送出错
		FeyinMessage formatedMessage = new FeyinMessage(
				FeyinMessageType.formatedMessage, memberCode, deviceNo,
				msgDetail);
		
		//以下参数并不是必须的，您可以根据需要设置
		formatedMessage.setCharge(charge);
		formatedMessage.setCustomerName(customerName);
		formatedMessage.setCustomerPhone(customerPhone);
		formatedMessage.setCustomerAddress(customerAddress);
		formatedMessage.setCustomerMemo(customerMemo);
		formatedMessage.setExtra1(extra1);
		formatedMessage.setMsgNo(msgNo);		//不设置将由飞印平台自动生成
		
		int returnVal = -10;
		try {
			returnVal = client.sendFormatedMessage(formatedMessage);
		} catch (Exception e) {
			returnVal = -10;
			System.err.println(e);
		}
		dealWithReturnValue(returnVal);
		
		return msgNo;
	}
	
	private static void testQueryState(FeyinClient client,String msgNo) {
		int content ;

		try {
			content = client.queryState(msgNo);
		} catch (Exception e) {
			e.printStackTrace();
			content = -10;
		}
		
		System.out.println("服务器返回：" + content);
	}


	private static void dealWithReturnValue(int returnVal) {
		if (returnVal == -10)
			System.out.println("服务器错误");
		else {
			System.out.println("服务器返回：" + returnVal);
		}
	}
}
