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

## 关于 JWT 中的签发则与订阅则

1. 签发者和订阅者可以随意定义,不一定得是 url,也可以是某个 id,只要校验的时候 jwt 中携带的信息能在预设的列表中就能通过验证

## 创建数据库脚本(SQLServer)

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
