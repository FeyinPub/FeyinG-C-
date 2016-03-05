package com.feyin.api;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.URI;
import java.util.ArrayList;
import java.util.List;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.utils.URIUtils;
import org.apache.http.client.utils.URLEncodedUtils;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.protocol.HTTP;

import com.feyin.util.MD5Ecnrypt;


/**
 * 飞印api
 * 
 * @author tommy.zeng
 * 
 */
public class FeyinClient {

	String host = "my.feyin.net";
	int port = 80;
	String apiKey;  //api 密钥，由feyin网提供
	String memberCode; //商户的代码，由feyin网提供

	public FeyinClient(String host, int port, String apiKey, String memberCode) {
		super();
		this.host = host;
		this.port = port;
		this.apiKey = apiKey;
		this.memberCode = memberCode;
	}

	

	/**
	 * 提交格式化的内容
	 * @param msg
	 * @return 详见api文档
	 * @throws Exception
	 */
	public int sendFormatedMessage(FeyinMessage msg) throws Exception {
		if (msg.getMessageType() != FeyinMessageType.formatedMessage)
			throw new Exception("wrong message type");

		long reqTime = System.currentTimeMillis();

		String contentToEncryt = String.format("%s%s%s%s%s%s%s%s%d%s",
				this.memberCode, msg.customerName, msg.customerPhone,
				msg.customerAddress, msg.customerMemo, msg.msgDetail,
				msg.deviceNo, msg.msgNo, reqTime, this.apiKey);
	
		String securityCode = MD5Ecnrypt.EncodeMD5Hex(contentToEncryt);

		List<NameValuePair> nvps = new ArrayList<NameValuePair>();

		nvps.add(new BasicNameValuePair("memberCode", memberCode));
		nvps.add(new BasicNameValuePair("charge", msg.charge));
		nvps.add(new BasicNameValuePair("customerName", msg.customerName));
		nvps.add(new BasicNameValuePair("customerPhone", msg.customerPhone));
		nvps.add(new BasicNameValuePair("customerAddress", msg.customerAddress));
		nvps.add(new BasicNameValuePair("customerMemo", msg.customerMemo));
		nvps.add(new BasicNameValuePair("msgDetail", msg.msgDetail));
		nvps.add(new BasicNameValuePair("deviceNo", msg.deviceNo));
		nvps.add(new BasicNameValuePair("msgNo", msg.msgNo));
		
		nvps.add(new BasicNameValuePair("extra1", msg.extra1));
		nvps.add(new BasicNameValuePair("extra2", msg.extra2));
		nvps.add(new BasicNameValuePair("extra3", msg.extra3));
		nvps.add(new BasicNameValuePair("extra4", msg.extra4));
		
		nvps.add(new BasicNameValuePair("reqTime", String.valueOf(reqTime)));

		nvps.add(new BasicNameValuePair("securityCode", securityCode));
		nvps.add(new BasicNameValuePair("mode", "1"));

		return sendMessage(nvps);

	}

	/**
	 * 提交自由格式的内容
	 * @param msg
	 * @return 详见api文档
	 * @throws Exception
	 */
	public int sendFreeMessage(FeyinMessage msg) throws Exception {

		if (msg.getMessageType() != FeyinMessageType.freeMessage)
			throw new Exception("wrong message type");

		long reqTime = System.currentTimeMillis();

		String contentToEncryt = String.format("%s%s%s%s%d%s", this.memberCode,
				msg.msgDetail, msg.deviceNo, msg.msgNo, reqTime, this.apiKey);

		String securityCode = MD5Ecnrypt.EncodeMD5Hex(contentToEncryt);

		List<NameValuePair> nvps = new ArrayList<NameValuePair>();

		nvps.add(new BasicNameValuePair("memberCode", memberCode));
		nvps.add(new BasicNameValuePair("msgDetail", msg.msgDetail));
		nvps.add(new BasicNameValuePair("deviceNo", msg.deviceNo));
		nvps.add(new BasicNameValuePair("msgNo", msg.msgNo));
		nvps.add(new BasicNameValuePair("reqTime", String.valueOf(reqTime)));
		
		nvps.add(new BasicNameValuePair("securityCode", securityCode));
		nvps.add(new BasicNameValuePair("mode", "2"));

		return sendMessage(nvps);

	}

	/**
	 * 查询异常打印列表 
	 * @return 详见api文档
	 * @throws Exception
	 */
	public String listException() throws Exception {
		long reqTime = System.currentTimeMillis();
		String contentToEncryt = String.format("%s%d%s", this.memberCode,
				reqTime, this.apiKey);

		String securityCode = MD5Ecnrypt.EncodeMD5Hex(contentToEncryt);

		List<NameValuePair> nvps = new ArrayList<NameValuePair>();

		nvps.add(new BasicNameValuePair("memberCode", memberCode));
		nvps.add(new BasicNameValuePair("reqTime", String.valueOf(reqTime)));

		nvps.add(new BasicNameValuePair("securityCode", securityCode));

		return sendGetMessage("/api/listException", nvps);
	}

	/**
	 * 查询设备列表
	 * @return 详见api文档
	 * @throws Exception
	 */
	public String listDevice() throws Exception {
		long reqTime = System.currentTimeMillis();
		String contentToEncryt = String.format("%s%d%s", this.memberCode,
				reqTime, this.apiKey);

		String securityCode = MD5Ecnrypt.EncodeMD5Hex(contentToEncryt);

		List<NameValuePair> nvps = new ArrayList<NameValuePair>();

		nvps.add(new BasicNameValuePair("memberCode", memberCode));
		nvps.add(new BasicNameValuePair("reqTime", String.valueOf(reqTime)));

		nvps.add(new BasicNameValuePair("securityCode", securityCode));

		return sendGetMessage("/api/listDevice", nvps);
	}

	/**
	 * 查询消息打印的状态
	 * @param msgNo
	 * @return  详见api文档
	 * @throws Exception
	 */
	public int queryState(String msgNo) throws Exception {
		long reqTime = System.currentTimeMillis();
		String contentToEncryt = String.format("%s%d%s%s", this.memberCode,
				reqTime, this.apiKey, msgNo);

		String securityCode = MD5Ecnrypt.EncodeMD5Hex(contentToEncryt);
		
		List<NameValuePair> nvps = new ArrayList<NameValuePair>();

		nvps.add(new BasicNameValuePair("memberCode", memberCode));
		nvps.add(new BasicNameValuePair("reqTime", String.valueOf(reqTime)));

		nvps.add(new BasicNameValuePair("securityCode", securityCode));
		nvps.add(new BasicNameValuePair("msgNo", msgNo));

		String result = sendGetMessage("/api/queryState", nvps);

		return Integer.parseInt(result);
	}

	/**
	 * 公共的发送post请求的方法
	 * @param nvps
	 * @return
	 * @throws Exception
	 */
	private int sendMessage(List<NameValuePair> nvps) throws Exception {
		int result = 0;
		HttpPost httppost = new HttpPost("http://" + this.host + ":"
				+ this.port + "/api/sendMsg");

		httppost.setEntity(new UrlEncodedFormEntity(nvps, HTTP.UTF_8));
		HttpClient httpclient = new DefaultHttpClient();

		HttpResponse response = httpclient.execute(httppost);
		HttpEntity resEntity = response.getEntity();
		int returnCode = response.getStatusLine().getStatusCode();

		if (returnCode == 200 || returnCode == 302) {
			BufferedReader br = new BufferedReader(new InputStreamReader(
					resEntity.getContent()));
			
			String readLine;
			if (((readLine = br.readLine()) != null)) {
				result = Integer.parseInt(readLine);
			}
		} else {
			System.out.println("faild");
			result = -10;
		}
		
		httpclient.getConnectionManager().shutdown();

		return result;
	}

	/**
	 * 公共的发送Get请求的方法
	 * @param url
	 * @param nvps
	 * @return
	 * @throws Exception
	 */
	private String sendGetMessage(String url, List<NameValuePair> nvps)
			throws Exception {
		String result = "";
		
		URI uri = URIUtils.createURI("http", this.host, this.port, url, 
				URLEncodedUtils.format(nvps, "UTF-8"), null);
		
		HttpGet httpget = new HttpGet(uri);
		HttpClient httpclient = new DefaultHttpClient();

		HttpResponse response = httpclient.execute(httpget);
		HttpEntity resEntity = response.getEntity();
		int returnCode = response.getStatusLine().getStatusCode();

		if (returnCode == 200 || returnCode == 302) {
			BufferedReader br = new BufferedReader(new InputStreamReader(
					resEntity.getContent()));
			
			String readLine;
			StringBuffer buf = new StringBuffer();
			while (((readLine = br.readLine()) != null)) {
				buf.append(readLine);
			}
			result = buf.toString();
		} else {
			System.out.println("faild");
			result = "-10";
		}

		httpclient.getConnectionManager().shutdown();

		return result;

	}

	
	public String getHost() {
		return host;
	}

	public void setHost(String host) {
		this.host = host;
	}

	public int getPort() {
		return port;
	}

	public void setPort(int port) {
		this.port = port;
	}

	public String getApiKey() {
		return apiKey;
	}

	public void setApiKey(String apiKey) {
		this.apiKey = apiKey;
	}

	public String getMemberCode() {
		return memberCode;
	}

	public void setMemberCode(String memberCode) {
		this.memberCode = memberCode;
	}
}
