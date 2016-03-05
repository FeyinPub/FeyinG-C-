package com.feyin.util;

import java.security.MessageDigest;

public class MD5Ecnrypt {
	// 将字符串加密成MD5，32位16进制字串，如"3031209"转成"e043a49740adde7aae4f34818c52528e"
	public static String EncodeMD5Hex(String text) throws Exception {
		MessageDigest md = MessageDigest.getInstance("MD5");
		md.update(text.getBytes("utf-8"));
		byte[] digest = md.digest();
		StringBuffer md5 = new StringBuffer();
		for (int i = 0; i < digest.length; i++) {
			md5.append(Character.forDigit((digest[i] & 0xF0) >> 4, 16));
			md5.append(Character.forDigit((digest[i] & 0xF), 16));
		}
		return md5.toString();
	}

	// 将字符串加密成ASCII字串，如"3031209"转成"?C??@??z?O4??RR?"
	public static String EncodeMD5ASCII(String text) throws Exception {
		MessageDigest md = MessageDigest.getInstance("MD5");
		md.update(text.getBytes("US-ASCII"));
		byte[] digest = md.digest();
		return new String(digest, "US-ASCII");
	}

	// 将ASCII字串编码成16进制字串,如：“?C??@??z?O4??RR?“转成”3f433f3f403f3f7a3f4f343f3f52523f“
	// 参数text
	// 返回结果16进制字符串
	public static String DecodeMD5Hex(String text) throws Exception {
		byte[] digest = text.getBytes();
		StringBuffer md5 = new StringBuffer();
		for (int i = 0; i < digest.length; i++) {
			md5.append(Character.forDigit((digest[i] & 0xF0) >> 4, 16));
			md5.append(Character.forDigit((digest[i] & 0xF), 16));
		}
		return md5.toString();
	}

	// 比较输入密码MD5加密后与数据库密码相等
	// 参数a:输入的密码
	// 参数b:数据库的ASCII字串
	// 返回结果：a进行md5加密后和b相等则为真，反之则为假
	public static Boolean CheckPSW(String a, String b) throws Exception {
		String strone = EncodeMD5ASCII(a);
		return strone.equals(b);
	}
}