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

# Ví dụ:
  1. gateway api (Yarp)
    POST http://localhost:5175/identity/api/auth/login
  2. identity api
     POST http://localhost:5117/api/auth/login

  3. postman
  
        POST http://localhost:5175/identity/api/auth/login: {
          "Network": {
            "addresses": {
              "local": {
                "address": "::1",
                "family": "IPv6",
                "port": 51387
              },
              "remote": {
                "address": "::1",
                "family": "IPv6",
                "port": 5175
              }
            }
          },
          "Request Headers": [
            {
              "key": "Content-Type",
              "value": "application/json",
              "system": true
            },
            {
              "key": "User-Agent",
              "value": "PostmanRuntime/7.39.1",
              "system": true
            },
            {
              "key": "Accept",
              "value": "*/*",
              "system": true
            },
            {
              "key": "Cache-Control",
              "value": "no-cache",
              "system": true
            },
            {
              "key": "Postman-Token",
              "value": "1e53b88c-48a4-466c-9608-f85ab52bc3ba",
              "system": true
            },
            {
              "key": "Host",
              "value": "localhost:5175",
              "system": true
            },
            {
              "key": "Accept-Encoding",
              "value": "gzip, deflate, br",
              "system": true
            },
            {
              "key": "Connection",
              "value": "keep-alive",
              "system": true
            },
            {
              "key": "Content-Length",
              "value": "134",
              "system": true
            }
          ],
          "Request Body": "\"{\\r\\n  \\\"email\\\": \\\"user@example.com\\\",\\r\\n  \\\"password\\\": \\\"YourPassword123!\\\",\\r\\n  \\\"twoFactorCode\\\": \\\"123456\\\",\\r\\n  \\\"twoFactorRecoveryCode\\\": null\\r\\n}\"",
          "Response Headers": [
            {
              "key": "Content-Type",
              "value": "application/json; charset=utf-8"
            },
            {
              "key": "Date",
              "value": "Wed, 01 Jul 2026 10:38:08 GMT"
            },
            {
              "key": "Server",
              "value": "Kestrel"
            },
            {
              "key": "Transfer-Encoding",
              "value": "chunked"
            }
          ],
          "Response Body": "{\"token\":\"jwt-token-test\"}"
        }




        POST http://localhost:5117/api/auth/login: {
          "Network": {
            "addresses": {
              "local": {
                "address": "::1",
                "family": "IPv6",
                "port": 52786
              },
              "remote": {
                "address": "::1",
                "family": "IPv6",
                "port": 5117
              }
            }
          },
          "Request Headers": [
            {
              "key": "Content-Type",
              "value": "application/json",
              "system": true
            },
            {
              "key": "User-Agent",
              "value": "PostmanRuntime/7.39.1",
              "system": true
            },
            {
              "key": "Accept",
              "value": "*/*",
              "system": true
            },
            {
              "key": "Cache-Control",
              "value": "no-cache",
              "system": true
            },
            {
              "key": "Postman-Token",
              "value": "0c8b081a-569c-4455-ae66-d8989d31512a",
              "system": true
            },
            {
              "key": "Host",
              "value": "localhost:5117",
              "system": true
            },
            {
              "key": "Accept-Encoding",
              "value": "gzip, deflate, br",
              "system": true
            },
            {
              "key": "Connection",
              "value": "keep-alive",
              "system": true
            },
            {
              "key": "Content-Length",
              "value": "134",
              "system": true
            }
          ],
          "Request Body": "\"{\\r\\n  \\\"email\\\": \\\"user@example.com\\\",\\r\\n  \\\"password\\\": \\\"YourPassword123!\\\",\\r\\n  \\\"twoFactorCode\\\": \\\"123456\\\",\\r\\n  \\\"twoFactorRecoveryCode\\\": null\\r\\n}\"",
          "Response Headers": [
            {
              "key": "Content-Type",
              "value": "application/json; charset=utf-8"
            },
            {
              "key": "Date",
              "value": "Wed, 01 Jul 2026 10:40:54 GMT"
            },
            {
              "key": "Server",
              "value": "Kestrel"
            },
            {
              "key": "Transfer-Encoding",
              "value": "chunked"
            }
          ],
          "Response Body": "{\"token\":\"jwt-token-test\"}"
        }