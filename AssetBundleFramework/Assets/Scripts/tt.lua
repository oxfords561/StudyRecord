
print('hello world') -- hello world

--print('hello world') 单行注释

--[[
print('演示多行注释')
print('演示多行注释')
]]

--全局变量
print("############# 全局变量 ###########")

print(b)

b=10
print(b)

b=nil
print(b) --删除了 变量 b

--数据类型
print("############# 数据类型 ###########")

print(type("hello world"))   -->string

print(type(10.4*3))          -->number

print(type(print))           -->function

print(type(type))            -->function

print(type(true))            -->boolean

print(type(nil))             -->nil

print(type(type(X)))         -->string

if false or nil then
    print("至少有个true")
else
    print("false 和 nil 都为 false")
end

str1 = "字符串"

str2 = "字符串2"

html = [[
    <html>
    <head></head>
    <body>
        <a href="http://www.baidu.com">haha</a>
    </body>
    </html>
]]

print(html)

print("2"+6)

print("2+6")

print("2"+"6")

print("hello".."world")

len = "字符串长度"
print(#len)

local tbl1 = {}

local tbl2 = {"1","2","3","4"}

tbl1["key"] = "value"
tbl1[2] = "3"

key = 10
tbl1[key] = 22
tbl1[key] = tbl1[key] + 11
for k,v in pairs(tbl1) do
    print(k..":"..v)
end

for key,val in pairs(tbl2) do
    print("key",key)
end

function func1(n)
    if n == 0 then
        return 1
    else
        return n * func1(n-1)
    end
end

print(func1(5))
func2 = func1
print(func2(5))

tab = {ke1="val1",key2="val2"}

for key,val in pairs(tab) do
    print(key.."="..val)
end
