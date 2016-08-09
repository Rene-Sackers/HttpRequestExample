local reflection = clr.System.Reflection
local assembly = reflection.Assembly.LoadFrom('resources/http-example/server/lib/FiveRebornHttp.dll')
local fiveRebornHttp = clr.FiveRebornHttp.HttpHandler

-- Gets content of page from http://pastebin.com/raw/8SK5pT0e 

local url = "http://pastebin.com/raw/8SK5pT0e" 
local callback = function (content, statusCode, responseHeaders)
    print("HTTP response received.")
    print("Content: " .. content)
    print("Status code: " .. statusCode)
    print("Headers: " .. json.encode(responseHeaders))
end
local method = "get"
local data = nil
local headers = { }

--[[
    == MakeRequest ==
    = Arguments =
    name        | type      | optional  | comments
    ------------|-----------|-----------|---------
    url         | string    | no        |
    callback    | function  | no        | Function definition: string: content, string: statusCode, table: response headers
    method      | string    | yes       | Possible options: delete, head, options, post, put, trace
    data        | string    | yes       | You can use "json.encode(mytablevariable)" to turn a Lua table into JSON
    headers     | table     | yes       | Use this to send extra headers, like { ["Content-Type"] = "application/json" }
]]--
-- Optional: method, data, headers
fiveRebornHttp.MakeRequest(url, callback, method, data, headers)