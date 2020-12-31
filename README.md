# QuickFrame 快速开发框架

> 该文档现在仅记录思考点,开发规范,踩坑记录等.没有考虑编写顺序,也没有做分类.待框架完善,稳定后,再详细编写文档说明框架的设计理念,以及代码的编写规范

## 首次运行项目

运行还原包命令`dotnet restore`

## 设计理念

1. 将业务层与数据层分离,让业务层没有数据库的概念.
2. 仓储层直接对接数据库,实现一系列具有原子性的数据库访问代码.例如:查询产品库存量,查询已销售数量,查询已开单数量,审核时修改标志,审核人,审核时间等等,具有不可拆分的逻辑.
3. 业务层(Service)负责组合调用仓储层的原子行为,来描述整体的业务逻辑.这就使得业务层不需要关心数据库的具体实现,仅描述一系列的原子行为即可组成一个完整的业务处理流程.去掉业务层数据库的概念,降低业务逻辑难度.
4. 全面使用依赖注入(DI)来降低对象与对象之间的耦合度.
5. 每编写一个 Service 和 Controller 都应该有对应的测试类,如果有必要,Common 层也可以写测试.(为了实现 DevOps CI\CD)
6. Service 应该遵循先定义行为(接口),描述一定要清晰,要具体到某一个行为. Controller 层应该直接依赖于 Service 层的行为,而不是实现类.
7. 一个仓储可以调用另一个仓储。同时应该避免仓储的相互依赖，如果出现了相互依赖的情况，不适用构造函数注入，而是使用 get;的方式进行注入.
8. 一个服务也可以调用另一个服务。循环依赖问题的解决方案和仓储一样.

## 实现目标

1. 完善 README.md,详细说明项目结构和使用方法.需要给出完整的开发步骤,以及 common 层每个类的作用
2. 完善日志系统.控制台日志,文件日志,数据库日志
3. 完善模型验证(使用 asp.net core 自带的模型验证) --完成
4. 完善 API 返回内容的统一 --完成
5. 简化 common 层的内容,有些不常用的库用到的时候再加上
6. 完善 swgger 的实体文档描述 --完成
7. 完善 AOP 面向切面编程内容 --完成
8. 完善权限验证,包括数据库的表
9. 完善 EFCore 的配置.(尽量使用代码优先的方式) --完成
10. 添加单元测试,集成测试.(包含 web,service,common) --完成
11. 写个迁移工具,将 wcf 的 service 层解析为.net core 中的 controller
12. 搭建 identityserver4,并完善 ui.并且由它统一签发 Token --ui 未完成
13. 实现前端传入条件 object 到 LambdaExpress 的转换 --完成
14. 规范和实现前端传入数据的签名及验证
15. 将 zxsc 所有实体迁移到项目中(有点费脑筋，估计得写工具)

## 实现计划

> 以下计划中只考虑迁移工作(包括账套的创建 API),不进行详细的重构

1. 将 WCF 中所有数据实体迁移到 Core 中
2. 将 WCF 中的 Service 迁移到 Controller 中
3. 将 WCF 中的 BLL 迁移到 Service 中
4. 完善 dotnet-zxtool 工具

> 实现字符串条件的查询能力 (原来的动态查询太臃肿,反正我没有找到详细的文档,不能完全掌控,就感觉不安全)

1. 大概实现思路:词法分析,语法分析,最终解析为 Lambda 表达式树，然后利用 IQueryable 进行查询

> 水晶报表貌似不支持 core 版的,得换成锐浪报表

1. 确认水晶报表和锐浪报表在 core 中是否可用
2. 如果可用则开始报表迁移,如果不可用则寻找替代方案

> 代码重构

1. 遵循面向对象的七大原则
2. 依赖注入尽量使用接口,用接口描述业务流程,而不是直接依赖于实现类
3. 关于是否接入 IdentityService4 进行讨论

> 项目部署

1. API 部署环境(本地开发时环境,测试环境,前端开发时环境,生产环境)
2. 数据库的部署
3. IdentityService4 的部署(如果启用)

> PS: 如果只是为了实现分布式,不用 IdentityService4 ,仅靠 jwt 就能实现.
>
> IdentityService4 的优势在于能够统一管理用户,并且支持单点登录,在存在多个子系统的架构中使用比较友好.而在单系统中使用反而显得不够简洁.
>
> 例如:我们的 zxsc 就算一个子系统,如果后期再有新的项目,就可以考虑将用户的信息统一在 sso 中管理,并且提供认证和授权的能力.

## 搭建 Gitblit

Gitblit 是一个远程代码仓库,搭建步骤已经有[相关的文档](https://www.cnblogs.com/ucos/p/3924720.html),这里不再赘述.

## 自动部署(DevOps)

> [如何实现 DevOps](https://www.cnblogs.com/stulzq/p/8629165.html)

### 技术点

1. 搭建 git 源代码管理仓库 gogs
2. 安装持续集成工具 Jenkins
3. 容器技术 docker

## EFCore Code 优先

这里只记录常用的命令。推荐使用方式二

### 方式一

> Visual Studio 环境下，就直接打开包管理控制台运行命令

```tex
//获取帮助文档
PM> get-help entityframework

//生成迁移文件
PM> add-migration Upd-context zxscdbcontext //其中 Upd 为描述,并且首字母尽量大写

//修改数据库
PM> update-database -context zxscdbcontext //其中 -context 作用是指定要运行的DbContext

//移除迁移文件
PM> remove-migration -context zxscdbcontext

//移除上一次迁移操作 //删除数据库
PM> drop-database -context zxscdbcontext //发生无法解决的错误时使用
```

### 方式二

> 如果在 VSCode 环境下，需要先安装`dotnet-ef`工具。(也适用于普通命令行)
>
> 安装命令：`dotnet tool install -g dotnet-ef`

```tex
//查看帮助
dotnet ef

//生成迁移文件命令
dotnet ef migrations add CreateDatabase --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model --output-dir Migrations/BackDb

//执行修改数据库命令
dotnet ef database update --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model

//执行移除迁移文件命令
dotnet ef migrations remove --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model

//执行删除数据库命令
dotnet ef database drop --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model
```

### 示例

```tex
dotnet ef migrations add CreateBackDatabase --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model --output-dir Migrations/BackDb
dotnet ef database update --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model
dotnet ef migrations remove --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model
dotnet ef database drop --context backdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model

dotnet ef migrations add CreateWorkDatabase --context workdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model --output-dir Migrations/WorkDb
dotnet ef database update --context workdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model
dotnet ef migrations remove --context workdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model
dotnet ef database drop --context workdbcontext --startup-project QuickFrame.Web --project QuickFrame.Model
```

```tex
dotnet ef database drop --context backdbcontext --project ../QuickFrame.Model -f
dotnet ef migrations add CreateBackDatabase --context backdbcontext --project ../QuickFrame.Model --output-dir Migrations/BackDb
dotnet ef migrations remove --context backdbcontext --project ../QuickFrame.Model
dotnet ef database update --context backdbcontext --project ../QuickFrame.Model

dotnet ef database drop --context workdbcontext --project ../QuickFrame.Model -f
dotnet ef migrations add CreateWorkDatabase --context workdbcontext --project ../QuickFrame.Model --output-dir Migrations/WorkDb
dotnet ef migrations remove --context workdbcontext --project ../QuickFrame.Model
dotnet ef database update --context workdbcontext --project ../QuickFrame.Model
```

## 反向工程

由于 zxsc 原先的数据库版本过低，与 EFCore 不兼容，所以无法完成自动反向工程。

> 反向工程使用到的命令：`dotnet ef dbcontext scaffold`

## 命名规范

### 一般规则

> 为避免过多的命名空间,增加维护难度

1. Web 层统一采用`QuickFrame.Web`
2. Repository 层统一使用命名空间`QuickFrame.Repository`
3. Common 层统一使用命名空间`QuickFrame.Common`(扩展类除外)
4. Model 层统一使用命名空间`QuickFrame.Model`
5. Controllers 层统一使用命名空间`QuickFrame.Controllers`
6. 扩展类的命名空间需要与扩展的类型的命名空间保持一致

### 特定规则

1. 关于是否使用 async/await 视情况而定。例如：需要等待异步方法返回结果后在执行的，就需要使用 async/await，否则就直接返回 Task 或 Task<>
2. 异步方法名称必须带有`Async`后缀

## 仓储层(Repository)

> 如果有特殊要求的场景可以继承`ZxscSysRepository`或`ZxscRepository`类,然后扩展自己的方法,尽量不修改已有的仓储

1. 每组单据都应该建立仓储,并且仓储层应该包含该单据的所有原子操作(不可拆分的操作,例如:)

> 关于工作单元

1. 由于 范围事务(TransactionScope) 在.net core 中的局限性.所以我们系统中不考虑使用这种方式.
2. 定义工作单元,工作单元仅具有开启事务,结束事务功能.
3. 定义工作单元管理器,负责实现 IDisplaced 接口,并且自动回滚功能.(方便使用 using 语句)

> 注: TransactionScope 的限制: 考虑到我们会跨服务器链接数据库,然而 TransactionScope 自.netcore2.0 之后就不在支持分布式事务,所以无法跨服务器使用.

## 关于事务

1. 事务中尽量只包含 CURD 操作,其余代码能尽量放在事务外部允许.事务独占资源,为尽快释放,与事务无关的代码尽量不放在事务中.

## 文件编码

1. cs 文件采用 utf-8-bom 编码方式
2. json 文件采用 utf-8 编码方式
3. csproj 文件采用 utf-8 编码方式

## 关于增删改查

### 查询

1. 一般情况下,多行查询都需要做分页

### 新增

1. 前端传入的实体不应该直接使用数据库表实体,而是使用 DTO 中间实体接收,然后再转给数据库实体,DTO 与前端进行绑定

### 修改

1. 如果没有特殊要求,可以与新增公用一个 DTO
2. 主表需要先查出来,然后将 DTO 的值覆盖上去,再保存到数据库
3. 子表不能先删了再建,先查出子表,然后对比删除的条目,然后将 DTO 值覆盖到子表,然后保存到数据库

### 删除

1. 一般按主键删除
2. 一般做级联删除

## 请求消息定义

1.示例一

```tex
http://api/system/userinfo/delete/123  //其中123为参数
```

2.示例二

> 这种一般会在参数过多的情况下使用

```tex
http://api/system/userinfo/delete?userid=123
```

3.示例三

> 放在 body 中的 json。一般用于创建和修改，条件查询
>
> condition 字段参照下文的《查询条件输入的设计》

```tex
{
    "condition":{},
    "page":{
        "index":1,
        "size":20,
        "sort":[
            {"orderby":"userid","desc":false},
            {"orderby":"createdate","desc":true},
        ]
    }
}
```

4.示例四

> 对于 新增,修改 这类接口直接传入对象的 json 即可,不需要额外在 json 中添加参数(账套,签名等信息,放在请求头中).具体传法参照接口文档(swagger)

## 查询条件设计

> 注: field 的值必须小写

### 示例一(常用)

1.代码 a

```json
{
    "logic": "and",
    "items": [
        { "field": "username", "value": "wyl", "compare": "contains" },
        { "field": "age", "value": 18, "compare": "less" },
        { "field": "username", "value": "admin", "compare": "contains" },
        { "field": "age", "value": 50, "compare": "lesseq" }
    ]
}
```

2.代码 b

```tex
username.contains('wyl') and age < 18 and username.contains('admin') and age <= 50
```

> 结果：a 与 b 等效

### 示例二(不常用)

1.代码 a

```json
{
    "logic": "or",
    "groups": [
        {
            "logic": "and",
            "items": [
                {
                    "field": "username",
                    "value": "wyl",
                    "compare": "contains"
                },
                { "field": "age", "value": 18, "compare": "less" }
            ]
        },
        {
            "logic": "or",
            "items": [
                {
                    "field": "username",
                    "value": "admin",
                    "compare": "contains"
                },
                { "field": "age", "value": 50, "compare": "lesseq" }
            ]
        }
    ]
}
```

2.代码 b

```tex
(username.contains('wyl') and age < 18) or (username.contains('admin') or age <= 50)
```

> 结果：a 与 b 等效

### 示例三(不常用)

1.代码 a

```json
{
    "logic": "and",
    "groups": [
        {
            "logic": "or",
            "items": [
                {
                    "field": "username",
                    "value": "wyl",
                    "compare": "contains"
                },
                { "field": "age", "value": 18, "compare": "less" }
            ]
        },
        {
            "logic": "or",
            "items": [
                {
                    "field": "username",
                    "value": "admin",
                    "compare": "contains"
                },
                { "field": "age", "value": 50, "compare": "lesseq" }
            ]
        }
    ]
}
```

2.代码 b

```tex
(username.contains('wyl') or age < 18) and (username.contains('admin') or age <= 50)
```

> 结果：a 与 b 等效

### 实现方法

1. 递归条件对象
2. 从最里层开始生成 lambda 表达式，并组装条件
3. 有点小难度

### 字符串查询条件

例如:`(username.contains('wyl') or age < 18) and (username.contains('admin') or age <= 50)`

1. 最外层的括号不是必须的
2. 比较符的左边必须是字段,右边必须是值
3. 支持的比较符分别为:`contanis`,`>`,`<`,`!=`,`>=`,`<=`,`=`
4. 支持的逻辑符分别为:`and`,`or`

## 请求数据签名

> 请求签名能有效的防止中间人修改请求数据，通过在请求中加入时间戳课可以防止请求重放(时间戳可以先不加)
>
> 注:只有消息体 Body 中的 json 需要签名,请求头中的参数不需要(也就是说,只有新增,更新操作需要签名)

签名存放在 Header 中.示例: sign:xxxxxx

## 账套

每个 API 请求都需要带上操作的账套.账套也放入 Header 中.示例: dbname:zxsc14

## 在 Controller 中使用 FromBody,FromForm,FromRole,FromQuery

1. 根据 ID 删除、根据 ID 查询时使用 FromRole
2. 创建、新增、条件查询时使用 FromBody(条件查询时需要将 API 设置为 POST 请求方式)
3. 当 Get 方法传入参数为实体时使用 FromQuery
4. 尽量不使用 FromForm

## 响应消息定义

### 操作成功

```json
{
    "code": 200,
    "message": "成功"
}
```

### 操作失败

```json
{
    "code": 400,
    "message": "失败",
    "msgdetail": ["产品编码错误", "单号xxxxx未审核"]
}
```

### 查询结果

1.示例一

```json
{
    "code": 200,
    "message": "成功",
    "data": [{}, {}, {}, {}]
}
```

2.示例二

```json
{
    "code": 200,
    "message": "成功",
    "data": {
        "key1": "value",
        "key2": "value",
        "key3": "value",
        "key4": "value"
    }
}
```

3.示例三

```json
{
    "code": 200,
    "message": "成功",
    "data": {
        "key1": {},
        "key2": "value",
        "key3": {},
        "key4": 12
    }
}
```

## 客户部署(账套的创建)

> WCF 版本的实现也是可行的

1. 执行数据库的备份与还原都是在数据库服务器上执行的。包括备份的库都在数据库服务器上
2. 唯一的缺点就是有 bak 文件产生，还需要知道 bak 文件所在

> 我的实现思路

1. WEB 端提供表结构的 SQL 脚本,保证表结构时刻最新.
2. 做一个 dotnet tool 命令行工具,调用 WEB 接口取到 SQL 脚本,并利用 SQLServer 自带的 sqlcmd 执行脚本创建数据库.(执行过程可以写成.bat 文件,能做到一键完成)
3. 基础数据用 Excel,JSON 进行保存,并且使用这个进行导入工具,让结构与数据分开

> 优点:

1. dotnet tool 工具安装非常方便,只有一条命令 `dotnet tool install -g dotnet-zxtool`(前提条件:安装了.net core sdk)
2. 不会有额外的文件产生

> 缺点:

1. WEB 端不能直接创建数据库.(如果只考虑 WEB 与 SQLServer 部署在同一台主机上,也可以实现)

> PS: 创数据库这样的操作怎么也得运维去做吧... !<^>

## Service 规范

1. public 修饰的方法提供给 WEB 层 Controller 使用,所以 public 方法尽量能够处理一次请求.具体到请求
2. internal 修饰的方法是提供给 Service 层调用(功能).具体到某一个功能.
3. private 修饰的方法作为内部复用方法,不对外开放.(单一职责,方法复用)
4. protect 在 Service 层这里不常用.

## Controller 规范

1. 对于不跨 Service 的调用,应该只调用该 Service 一次就应该满足返回结果.
2. 对于跨 Service 的调用,在 Controller 中进行组合.
3. 每个 Service 应该做到,仅调用一次.

!> 遵循规范,可以降低业务复杂度

## dotnet 命令

[官方文档](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet)

### 更新包到最新版

```tex
//更新`QuickFrame.Common`项目中的`Autofac.Extensions.DependencyInjection`到最新版
dotnet add QuickFrame.Common package Autofac.Extensions.DependencyInjection
//还能指定源
dotnet add QuickFrame.Common package Autofac.Extensions.DependencyInjection -s https://api.nuget.org/v3/index.json
```

### 简化开发测试发布

1. 需要部署内部的 nuget 包管理器,或者在`nuget.org`官网开个账户,用于存放包
2. 项目的 `Controllers`,`Service`,`Repoository`,`Tests`,`Model` 都能够按模块拆成单独的包,独立维护.
3. 主项目只需要引用这些包即可完成发布,包的更新也非常方便,只需要将包的跟新命令写成一个批处理文件,然后一键更新,测试,发布

## 测试条件查询接口数据

```json
{
    "condition": {
        "logic": "and",
        "items": [
            {
                "field": "scy_ccompanyid",
                "value": "112",
                "compare": "contains"
            },
            {
                "field": "scy_ccompanyname",
                "value": "xxx",
                "compare": "contains"
            }
        ]
    },
    "page": {
        "index": 1,
        "size": 20,
        "sort": [
            {
                "orderBy": "scy_ccompanyid",
                "desc": false
            },
            {
                "orderBy": "scy_ccompanyname",
                "desc": true
            }
        ]
    }
}
```

## 关于 JWT 中的签发则与订阅则

1. 签发者和订阅者可以随意定义,不一定得是 url,也可以是某个 id,只要校验的时候 jwt 中携带的信息能在预设的列表中就能通过验证

## 创建数据库(SQLServer)

```sql
USE master
GO
CREATE DATABASE zxsc14 ON PRIMARY
(
    NAME='zxsc14',--主文件逻辑文件名
    FILENAME='D:\MyLocalDB\zxsc14.mdf', --主文件文件名
    SIZE=5mb,--系统默认创建的时候会给主文件分配初始大小
    MAXSIZE=500MB,--主文件的最大值
    filegrowth=15%-- 主文件的增长幅度
)
LOG ON
(
    name='zxsc14_log',--日志文件逻辑文件名
    filename='D:\MyLocalDB\zxsc14_log.ldf',--日志文件屋里文件名
    SIZE=5MB,--日志文件初始大小
    filegrowth=0 --启动自动增长
)
GO
```
