package com.feyin.api;
/**
 * 飞印的消息
 * @author tommy
 */
public class FeyinMessage {
	FeyinMessageType messageType;
	
	String memberCode;
	String charge;
	String customerName;
	String customerPhone;
	String customerAddress;
	String customerMemo;
	String msgDetail;
	String deviceNo;
	String msgNo;
	
	String extra1;
	String extra2;
	String extra3;
	String extra4;
	
	public FeyinMessage() {
		super();
	}
	
	public FeyinMessage(FeyinMessageType messageType,String memberCode, String deviceNo, String msgDetail) {
		super();
		this.messageType = messageType;
		this.memberCode = memberCode;
		this.msgDetail = msgDetail;
		this.deviceNo = deviceNo;
		
		this.charge = "";
		this.customerName = "";
		this.customerPhone = "";
		this.customerAddress = "";
		this.customerMemo = "";
		this.msgNo = "";
		this.extra1 = "";
		this.extra2 = "";
		this.extra3 = "";
		this.extra4 = "";
	}
	public String getMemberCode() {
		return memberCode;
	}
	public void setMemberCode(String memberCode) {
		this.memberCode = memberCode;
	}
	public String getCharge() {
		return charge;
	}
	public void setCharge(String charge) {
		this.charge = charge;
	}
	public String getCustomerName() {
		return customerName;
	}
	public void setCustomerName(String customerName) {
		this.customerName = customerName;
	}
	public String getCustomerPhone() {
		return customerPhone;
	}
	public void setCustomerPhone(String customerPhone) {
		this.customerPhone = customerPhone;
	}
	public String getCustomerAddress() {
		return customerAddress;
	}
	public void setCustomerAddress(String customerAddress) {
		this.customerAddress = customerAddress;
	}
	public String getCustomerMemo() {
		return customerMemo;
	}
	public void setCustomerMemo(String customerMemo) {
		this.customerMemo = customerMemo;
	}
	public String getMsgDetail() {
		return msgDetail;
	}
	public void setMsgDetail(String msgDetail) {
		this.msgDetail = msgDetail;
	}
	public String getDeviceNo() {
		return deviceNo;
	}
	public void setDeviceNo(String deviceNo) {
		this.deviceNo = deviceNo;
	}
	public String getMsgNo() {
		return msgNo;
	}
	public void setMsgNo(String msgNo) {
		this.msgNo = msgNo;
	}
	
	public String getExtra1() {
		return extra1;
	}
	public void setExtra1(String extra1) {
		this.extra1 = extra1;
	}
	public String getExtra2() {
		return extra2;
	}
	public void setExtra2(String extra2) {
		this.extra2 = extra2;
	}
	public String getExtra3() {
		return extra3;
	}
	public void setExtra3(String extra3) {
		this.extra3 = extra3;
	}
	public String getExtra4() {
		return extra4;
	}
	public void setExtra4(String extra4) {
		this.extra4 = extra4;
	}

	public FeyinMessageType getMessageType() {
		return messageType;
	}

	public void setMessageType(FeyinMessageType messageType) {
		this.messageType = messageType;
	}
	
	
}
