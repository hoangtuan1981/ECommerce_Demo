# Giải thích file appsettings.json của project

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ReverseProxy": {
    "Routes": {
      "identity": {
        "ClusterId": "identity-cluster",
        "Match": {
          "Path": "/identity/{**catch-all}"
        }
      }
    }
  },
  "Clusters": {
    "identity-cluster": {
      "Destinations": {
        "d1": {
          "Address": "https://localhost:5001/"
        }
      }
    }
  }

1. Routes
"Routes": {
    
}
Chứa danh sách các route mà API Gateway sẽ xử lý.
Mỗi route định nghĩa:

Request nào sẽ được match.
Request đó sẽ được forward đến cluster nào.

2. identity
"identity": {
    
}
Đây là:

Tên định danh của route.
Tự đặt, không bắt buộc phải là identity.
YARP chỉ dùng key này để quản lý cấu hình route.

3. ClusterId
"ClusterId": "identity-cluster"
Cho biết route này sẽ forward request đến cluster nào.
Ví dụ:
"ClusterId": "identity-cluster"
thì YARP sẽ tìm:
"Clusters": {
    "identity-cluster": {
        ...
    }
}

4. Match
"Match": {
    "Path": "/identity/{**catch-all}"
}
Định nghĩa điều kiện để route được chọn.
Ở đây:
/identity/{**catch-all}
có nghĩa:
Match tất cả request bắt đầu bằng:
/identity/
Ví dụ:
✅ Match
/identity/login
/identity/register
/identity/api/users
/identity/api/users/1

5. {**catch-all}
Đây là ASP.NET Core Route Pattern.
{**catch-all}
nghĩa là:
Bắt toàn bộ phần còn lại của URL.
catch-all = api/user/profile
GET /identity/login --> catch-all = login

6. Tóm lại 
  Identity API chỉ cần expose

  https://localhost:5001

  Gateway sẽ forward

  https://localhost:7000/identity/api/auth/login
  ↓
  https://localhost:5001/api/auth/login



# Tại sao thường đặt tên route là identity?
Trong mô hình Microservices:

ateway
 ├── /identity/*  -> Identity Service
 ├── /product/*   -> Product Service
 ├── /order/*     -> Order Service
 └── /payment/*   -> Payment Service
Config thường như sau:
"Routes": {
  "identity": {
    "ClusterId": "identity-cluster",
    "Match": {
      "Path": "/identity/{**catch-all}"
    }
  },
  "product": {
    "ClusterId": "product-cluster",
    "Match": {
      "Path": "/product/{**catch-all}"
    }
  }
}
