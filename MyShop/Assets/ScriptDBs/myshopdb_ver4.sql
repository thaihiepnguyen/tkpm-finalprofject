USE [MyShopDB]
GO
/****** Object:  Table [dbo].[category]    Script Date: 4/22/2023 6:34:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[CatID] [int] IDENTITY(1,1) NOT NULL,
	[CatName] [nvarchar](50) NULL,
	[CatIcon] [nvarchar](50) NULL,
	[CatDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_category] PRIMARY KEY CLUSTERED 
(
	[CatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[customer]    Script Date: 4/22/2023 6:34:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customer](
	[CusID] [int] IDENTITY(1,1) NOT NULL,
	[CusName] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_detail]    Script Date: 4/22/2023 6:34:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_detail](
	[OrderID] [int] NOT NULL,
	[ProID] [int] NOT NULL,
	[Quantity] [int] NULL,
	[TotalPrice] [money] NULL,
 CONSTRAINT [PK_order_detail] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ProID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product]    Script Date: 4/22/2023 6:34:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[ProID] [int] IDENTITY(1,1) NOT NULL,
	[ProName] [nvarchar](150) NULL,
	[Ram] [float] NULL,
	[Rom] [int] NULL,
	[ScreenSize] [float] NULL,
	[TinyDes] [nvarchar](300) NULL,
	[FullDes] [nvarchar](1000) NULL,
	[Price] [money] NULL,
	[ImagePath] [text] NULL,
	[Trademark] [text] NULL,
	[BatteryCapacity] [int] NULL,
	[CatID] [int] NULL,
	[Quantity] [int] NULL,
	[Block] [int] NULL,
 CONSTRAINT [PK_product] PRIMARY KEY CLUSTERED 
(
	[ProID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[purchase]    Script Date: 4/22/2023 6:34:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[purchase](
	[OrderID] [int] NULL,
	[CusID] [int] NULL,
	[CreatedAt] [date] NULL,
	[FinalTotal] [money] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[category] ON 

INSERT [dbo].[category] ([CatID], [CatName], [CatIcon], [CatDescription]) VALUES (1, N'Android', N'Android', N'Các thiết bị điện thoại chạy hệ điều hành Android')
INSERT [dbo].[category] ([CatID], [CatName], [CatIcon], [CatDescription]) VALUES (2, N'Iphone', N'Apple', N'Các thiết bị điện thoại chạy hệ điều hành IOS của hãng Apple')
INSERT [dbo].[category] ([CatID], [CatName], [CatIcon], [CatDescription]) VALUES (11, N'Window Phone', N'Windows', N'Các thiết bị điện thoại của Microsoft')
INSERT [dbo].[category] ([CatID], [CatName], [CatIcon], [CatDescription]) VALUES (13, N'điện thoại cục gạch', N'MobilePhone', N'Các thiết bị điện thoại cứng như đá')
SET IDENTITY_INSERT [dbo].[category] OFF
GO
SET IDENTITY_INSERT [dbo].[product] ON 

INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3013, N'Samsung Galaxy S23', 6, 256, 6.1, N'Samsung Galaxy S23 5G 128GB chắc hẳn không còn là cái tên 
quá xa lạ đối với các tín độ công nghệ hiện nay, được xem là một trong những gương mặt chủ 
chốt đến từ nhà Samsung với cấu hình mạnh mẽ bậc nhất, camera trứ danh hàng đầu cùng 
lối hoàn thiện vô cùng sang trọng và hiện đại.', NULL, 20990000.0000, N'Assets/Images/sp/7d9c9403-abba-41e0-88c2-f55785a41f0e.png', N'Samsung', 3900, 1, 2, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3014, N'iPhone 14 Pro Max', 6, 256, 6.7, N'iPhone 14 Pro Max một siêu phẩm trong giới smartphone được nhà Táo tung ra thị trường vào tháng 09/2022. Máy trang bị con chip Apple A16 Bionic vô cùng mạnh mẽ, đi kèm theo đó là thiết kế hình màn hình mới, hứa hẹn mang lại những trải nghiệm đầy mới mẻ cho người dùng iPhone.', NULL, 27290000.0000, N'Assets/Images/sp/3014.png', N'Apple', 4323, 2, 30, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3015, N'iPhone 14 Pro', 6, 256, 6.7, N'iPhone 14 Pro 128GB - Mẫu smartphone đến từ nhà Apple được mong đợi nhất năm 2022, lần này nhà Táo mang 
đến cho chúng ta một phiên bản với kiểu thiết kế hình notch mới, cấu hình mạnh mẽ nhờ con chip Apple A16 
Bionic và cụm camera có độ phân giải được nâng cấp lên đến 48 MP.', NULL, 25290000.0000, N'Assets/Images/sp/3015.png', N'Apple', 3200, 2, 10, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3016, N'Samsung Galaxy S21 FE', 6, 128, 6.4, N'Samsung Galaxy S21 FE 5G (6GB/128GB) được Samsung ra mắt với dáng dấp thời thượng, cấu hình khỏe,
chụp ảnh đẹp với bộ 3 camera chất lượng, thời lượng pin
đủ dùng hằng ngày và còn gì nữa? Mời bạn khám phá qua 
nội dung sau ngay.', NULL, 10990000.0000, N'Assets/Images/sp/3016.png', N'Samsung', 4500, 1, 1, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3017, N'Xiaomi Redmi 12C', 4, 64, 6.71, N'Xiaomi Redmi 12C 64GB là một thiết bị di động tầm trung được
giới thiệu bởi Xiaomi, với cấu hình vượt trội 
so với các đối thủ trong cùng phân khúc', NULL, 2890000.0000, N'Assets/Images/sp/bed11564-52bb-4c7e-b670-fe6034630f88.png', N'Xiaomi', 5000, 1, 5, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3018, N'Samsung Galaxy S23 Ultra', 8, 128, 6.8, N'Cuối cùng thì chiếc điện thoại Samsung Galaxy S23
cũng đã chính thức ra mắt tại sự kiện Galaxy Unpacked 
vào đầu tháng 2 năm 2023, máy nổi bật với camera 200 MP 
chất lượng, hiệu năng mạnh mẽ.', NULL, 26990000.0000, N'Assets/Images/sp/3018.png', N'Samsung', 5000, 1, 3, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3019, N'iPhone 11 64GB', 4, 6, 6.1, N'Apple đã chính thức trình làng bộ 3 siêu phẩm iPhone 11, trong đó phiên bản iPhone 11 64GB có mức giá rẻ nhất nhưng vẫn được nâng
cấp mạnh mẽ như iPhone Xr ra mắt trước đó.', NULL, 10590000.0000, N'Assets/Images/sp/3019.png', N'Apple', 3110, 2, 10, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3020, N'Điện thoại Vivo Y35', 8, 128, 6.58, N'Tiếp nối sự thành công của các mẫu smartphone tầm trung tại thị trường Việt Nam thì mới đây Vivo đã chính thức cho ra mắt điện thoại Vivo Y35. 
Máy sở hữu cho mình khả năng sạc siêu nhanh 44 W', NULL, 6290000.0000, N'Assets/Images/sp/3020.png', N'Vivo', 5000, 1, 4, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3021, N'OPPO Reno8 T 5G', 8, 64, 6.7, N'Tiếp nối sự thành công rực rỡ đến từ các thế hệ trước đó thì chiếc OPPO Reno8 T 5G 256GB cuối cùng
đã được giới thiệu chính thức tại Việt Nam', NULL, 10990000.0000, N'Assets/Images/sp/3021.png', N'OPPO', 4800, 1, 1, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3022, N'Samsung Galaxy A23', 4, 128, 6.6, N'Samsung Galaxy A23 4GB sở hữu bộ thông số kỹ thuật khá ấn tượng trong phân khúc, máy có một hiệu năng ổn định, cụm 4 camera thông minh cùng một diện mạo trẻ 
trung phù hợp cho mọi đối tượng người dùng.', NULL, 4790000.0000, N'Assets/Images/sp/3022.png', N'Samsung', 5000, 1, 10, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3025, N'Samsung Galaxy S20 FE', 8, 256, 6.5, N'Samsung đã giới thiệu đến người dùng thành viên mới của dòng S20 Series đó chính là điện thoại Samsung Galaxy S20 FE. Đây là mẫu flagship cao cấp quy tụ nhiều tính năng mà Samfan yêu thích, hứa hẹn sẽ mang lại trải nghiệm cao cấp của dòng Galaxy S với mức giá dễ tiếp cận hơn.', NULL, 8650000.0000, N'Assets/Images/sp/3025.png', N'Samsung', 4500, 1, 20, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3026, N'iPhone 14 128GB', 6, 128, 6.1, N'iPhone 14 128GB được xem là mẫu smartphone bùng nổ của nhà táo trong năm 2022, ấn tượng với ngoại hình trẻ trung, màn hình chất lượng đi kèm với những cải tiến về hệ điều hành và thuật toán xử lý hình ảnh, giúp máy trở thành cái tên thu hút được đông đảo người dùng quan tâm tại thời điểm ra mắt.', NULL, 19590000.0000, N'Assets/Images/sp/3026.png', N'Apple', 3279, 2, 30, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3027, N'iPhone 14 Pro Max', 6, 128, 6.6999998092651367, N'iPhone 14 Pro Max một siêu phẩm trong giới smartphone được nhà Táo tung ra thị trường vào tháng 09/2022', NULL, 27090000.0000, N'Assets/Images/sp/2d6cb8fe-710f-40f6-b749-fb8ed4d2b3ae.png', N'Apple', 4323, 2, 50, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3028, N'Samsung Galaxy A34', 8, 256, 6.5999999046325684, N'Samsung Galaxy A34 5G là mẫu điện thoại thông minh tầm trung mới của Samsung được ra mắt vào tháng 03/2023', NULL, 8630000.0000, NULL, N'Samsung', 5000, 1, 30, 0)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3029, N'iPhone 14 Pro Max', 6, 128, 6.6999998092651367, N'iPhone 14 Pro Max một siêu phẩm trong giới smartphone được nhà Táo tung ra thị trường vào tháng 09/2022', NULL, 27090000.0000, NULL, N'Apple', 4323, 2, 50, 1)
INSERT [dbo].[product] ([ProID], [ProName], [Ram], [Rom], [ScreenSize], [TinyDes], [FullDes], [Price], [ImagePath], [Trademark], [BatteryCapacity], [CatID], [Quantity], [Block]) VALUES (3030, N'Samsung Galaxy A34', 8, 256, 6.5999999046325684, N'Samsung Galaxy A34 5G là mẫu điện thoại thông minh tầm trung mới của Samsung được ra mắt vào tháng 03/2023', NULL, 8630000.0000, NULL, N'Samsung', 5000, 1, 30, 1)
SET IDENTITY_INSERT [dbo].[product] OFF
GO
ALTER TABLE [dbo].[product]  WITH CHECK ADD  CONSTRAINT [FK_product_category] FOREIGN KEY([CatID])
REFERENCES [dbo].[category] ([CatID])
GO
ALTER TABLE [dbo].[product] CHECK CONSTRAINT [FK_product_category]
GO
