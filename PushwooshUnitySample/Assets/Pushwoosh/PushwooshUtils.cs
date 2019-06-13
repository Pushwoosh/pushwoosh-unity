using UnityEngine;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;

public static class PushwooshUtils
{
	/// <summary>
	/// Parses json object and returns dictionary representation of json
	/// </summary>
	/// <returns>dictionary</returns>
	/// <exception cref="Exception">Thrown if json has bad format or it is not JSONObject</exception>
	/// <param name="json">JSON</param>
	public static IDictionary<string, object> JsonToDictionary(string json)
	{
		JSONObject jsonObject = JSON.Parse(json) as JSONObject;
		return JsonObjectToDictionary (jsonObject);
	}

	private static IDictionary<string, object> JsonObjectToDictionary(JSONObject jsonObject)
	{
		var result = new Dictionary<string, object> ();

		foreach (KeyValuePair<string, JSONNode> pair in jsonObject) {
			string key = pair.Key;
			JSONNode value = pair.Value;
			result.Add (key, JsonNodeToPOCO (value));
		}

		return result;
	}

	private static List<object> JsonArrayToList(JSONArray jsonArray)
	{
		List<object> result = new List<object> ();
		foreach (JSONNode node in jsonArray) {
			result.Add (JsonNodeToPOCO(node));
		}
		return result;
	}	

	private static object JsonNodeToPOCO(JSONNode node)
	{
		if (node.IsNumber) {
			return node.AsDouble;
		} 
		else if (node.IsBoolean) {
			return node.AsBool;
		} 
		else if (node.IsString) {
			return node.Value;
		} 
		else if (node.IsObject) {
			return JsonObjectToDictionary (node as JSONObject);
		} 
		else if (node.IsArray) {
			return JsonArrayToList (node as JSONArray);
		} 
		else if (node.IsNull) {
			return null;
		}

		throw new Exception ("Unrecognized json node type");
	}

	public static string DictionaryToJson(IDictionary<string, object> dictionary)
	{
		var entries = new List<string>();
		if (dictionary != null) {
			foreach (var attribute in dictionary) {	
				string key = attribute.Key;
				string value = POCOToJson(attribute.Value);

				entries.Add (string.Format ("\"{0}\": {1}", key, value));
			}
		}
		string result = "{" + string.Join(",", entries.ToArray()) + "}";
		return result;
	}

	private static string POCOToJson(object value)
	{
		if (value is string) {
			return "\"" + value + "\"";
		} 
		else if (value is IDictionary) {
			return DictionaryToJson(value as IDictionary<string, object>);
		} 
		else if (value is List<object>) {
			return ListToJson (value as List<object>);
		} 
        else if (value is DateTime) {
            return "\"" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm") + "\"";
        } 
        else {
			return "\"" + value.ToString() + "\"";
		}
	}

	private static string ListToJson(List<object> list)
	{
		var entries = new List<string> ();
		foreach(object o in list) {
			entries.Add (POCOToJson (o));
		}
		return "[" + string.Join(",", entries.ToArray()) + "]";
	}
}