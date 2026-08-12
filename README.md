# ⚡ 1-Click DNS Changer & Anti-Sanction Tester (2026)

[![Windows](https://img.shields.io/badge/OS-Windows%2010%20%2F%2011-blue.svg)](https://microsoft.com)
[![PowerShell](https://img.shields.io/badge/PowerShell-5.1%2B-blue.svg)](https://microsoft.com/powershell)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Status](https://img.shields.io/badge/Status-Active-brightgreen.svg)](https://github.com/Hadooshi/Iran-DNS-Toolbox)

---

## 🇮🇷 راهنمای فارسی (Persian Documentation)

### 🌟 درباره پروژه
**1-Click DNS Changer & Anti-Sanction Tester** یک ابزار هوشمند، قدرتمند و فوق‌العاده سریع برای سیستم‌عامل ویندوز است. این ابزار به صورت همزمان بیش از **۲۷ سرور DNS** ایرانی (رفع تحریم و کاهش پینگ گیمینگ) و بین‌المللی را تست کرده و پینگ واقعی و پاسخگویی پورت ۵۳ را سنجش می‌کند.

با استفاده از این برنامه می‌توانید تنها با **یک کلیک (وارد کردن عدد ۱ تا ۲۷)**، DNS دلخواه خود را مستقیماً روی **کارت شبکه فیزیکی سیستم (Wi-Fi یا کارت شبکه اصلی)** اعمال کنید و کارت‌های مجازی VPN (مانند NordLynx) را نادیده بگیرید.

---

### ⚠️ توجه بسیار مهم (حتماً بخوانید)
> تغییر DNS **آی‌پی عمومی (Public IP) شما را عوض نمی‌کند**؛ بلکه فقط تحریم‌هایی را دور می‌زند که بر اساس **DNS یا لوکیشن CDN** اعمال می‌شوند (مانند Adobe، Nvidia، Google Developer، Unity و...).
> اگر سرویسی دقیقاً IP شما را بررسی می‌کند، DNS به تنهایی کافی نیست و به VPN نیاز دارید.

---

### ✨ ویژگی‌های اصلی
* **⚡ تست موازی فوق‌العاده سریع (Parallel Runspace):** سنجش تمام ۲۷ سرور تنها در ۲ ثانیه!
* **🛡️ برچسب‌گذاری اختصاصی تحریم‌شکن‌ها:** مشخص کردن سرورهای اختصاصی رفع تحریم ایران (`[Anti-Sanction]`) نظیر شکن، رادار گیم، الکترو، ۴۰۳ آنلاین، بگذر و...
* **🔍 تست دوگانه (پینگ ICMP + پورت ۵۳ DNS):** سنجش واقعی اینکه آیا ISP شما DNS را فیلتر/مختل کرده یا خیر.
* **🎯 هدف‌گیری دقیق کارت شبکه فیزیکی:** شناسایی کارت شبکه اصلی سیستم (مانند وای‌فای `Realtek RTL8852BE`) و نادیده گرفتن تونل‌های VPN.
* **🔎 گزینه‌ی تایید مستقیم سیستم‌عامل `[V]`:** استخراج مستقیم آدرس DNS فعال از ویندوز و تست زنده حل دامنه‌های تحریم‌شده (مانند `developer.android.com`).
* **🔄 بازگردانی سریع به حالت خودکار `[R]`:** تنظیم مجدد DNS روی حالت پیش‌فرض DHCP.
* **🧹 پاک‌سازی کش سیستم `[F]`:** اجرای اتوماتیک `ipconfig /flushdns`.
* **👑 ارتقای اتوماتیک سطح دسترسی Administrator:** درخواست خودکار دسترسی ادمین جهت اعمال بدون خطای تغییرات شبکه.

---

### 🚀 نحوه نصب و اجرا

1. مخزن را کلون کنید یا فایل‌های **`DNS_Changer.bat`** و **`DNS_Changer.ps1`** را دانلود کنید:
   ```bash
   git clone https://github.com/Hadooshi/Iran-DNS-Toolbox.git
   ```
2. روی فایل **`DNS_Changer.bat`** دابل‌کلیک کنید.
3. در پنجره بازشده، شماره DNS مورد نظر (از **`1`** تا **`27`**) را تایپ کرده و Enter بزنید!

---

### 📊 منوی دستورات سریع
| کلید | دستور | عملکرد |
| :---: | :--- | :--- |
| **`1 - 27`** | **تغییر ۱-کلیکی** | اعمال آنی DNS انتخابی روی کارت شبکه فیزیکی وای‌فای/اترنت |
| **`V`** | **تایید وضعیت سیستم** | بررسی مستقیم از ویندوز + تست زنده حل دامنه‌های تحریم‌شده |
| **`0`** | **تنظیم خودکار** | ست کردن اتوماتیک سریع‌ترین DNS پیدا شده در تست پینگ |
| **`R`** | **ریست DNS** | بازگرداندن تنظیمات شبکه به حالت خودکار (DHCP) |
| **`F`** | **پاک‌سازی کش** | اجرای دستور `ipconfig /flushdns` |
| **`T`** | **تست مجدد** | تکرار تست پینگ و پورت ۵۳ برای تمام سرورها |
| **`Q`** | **خروج** | بستن برنامه |

---

### 📋 جدول سرورهای DNS پشتیبانی‌شده

#### 🟢 سرورهای رفع تحریم ایران (Anti-Sanction):
| `#` | نام سرویس | آی‌پی Primary / Secondary | برچسب | کاربری اصلی |
| :---: | :--- | :--- | :---: | :--- |
| **`1`** | **شکن (Shecan)** | `178.22.122.100` / `185.51.200.2` | `[Anti-Sanction]` | رفع تحریم عمومی وبسایت‌ها و لانچرها |
| **`2`** | **رادار گیم (Radar Game)** | `10.202.10.10` / `10.202.10.11` | `[Anti-Sanction]` | کاهش پینگ و رفع اختلال بازی‌ها (Xbox/PC) |
| **`3`** | **الکترو (Electro)** | `78.157.42.100` / `78.157.42.101` | `[Anti-Sanction]` | مچمیکینگ استیم، پلی‌استیشن و بازی‌های آنلاین |
| **`4`** | **بگذر سرور ۱ (Begzar 1)** | `185.55.226.26` / `185.55.225.25` | `[Anti-Sanction]` | دانلود بازی و لودینگ وب PC |
| **`5`** | **بگذر سرور ۲ (Begzar 2)** | `185.55.224.24` / `185.55.226.26` | `[Anti-Sanction]` | سرور جایگزین بگذر برای دانلود بازی |
| **`6`** | **۴۰۳ آنلاین (403 Online)** | `10.202.10.202` / `10.202.10.102` | `[Anti-Sanction]` | رفع تحریم ابزارها و کتابخانه‌های برنامه‌نویسی |
| **`7`** | **وانیلا (Vanilla)** | `10.139.177.21` / `10.139.177.22` | `[Anti-Sanction]` | سرویس‌های هوش مصنوعی، دانلود و بازی‌های آنلاین |
| **`8`** | **هاست ایران ۱ (HostIran 1)** | مطابق صفحه رسمی آنتی‌تحریم هاست ایران | `[Anti-Sanction]` | وبسایت‌ها و اپلیکیشن‌های خارجی |
| **`9`** | **هاست ایران ۲ (HostIran 2)** | مطابق صفحه رسمی آنتی‌تحریم هاست ایران | `[Anti-Sanction]` | وبسایت‌ها و اپلیکیشن‌های خارجی |
| **`10`** | **شلتر (Shelter)** | آی‌پی اختصاصی هر پلن (نمونه: `94.103.125.157`) | `[Anti-Sanction]` | پل ارتباطی بازی‌های آنلاین و کاهش پینگ |
| **`11`** | **بشکن (Beshkan)** | `181.41.194.177` / `181.41.194.186` | `[Anti-Sanction]` | رفع تحریم Adobe, Nvidia, Unity, Intel |

#### 🌐 سرورهای بین‌المللی (Global):
| `#` | نام سرویس | آی‌پی Primary / Secondary | توضیحات |
| :---: | :--- | :--- | :--- |
| **`12`** | **Cloudflare** | `1.1.1.1` / `1.0.0.1` | سریع‌ترین پاسخگویی جهانی |
| **`13`** | **Google Public DNS** | `8.8.8.8` / `8.8.4.4` | پایداری و سازگاری حداکثری |
| **`14`** | **Level3 (Lumen)** | `4.2.2.4` / `4.2.2.2` | پایداری مسیریابی بین‌المللی |
| **`15`** | **OpenDNS (Cisco)** | `208.67.222.222` / `208.67.220.220` | امنیت شبکه |
| **`16`** | **Quad9** | `9.9.9.9` / `149.112.112.112` | حریم خصوصی و ضد فیشینگ |
| **`17`** | **NTT Asia** | `129.250.35.250` | مسیریابی سرورهای آسیا |
| **`18`** | **Level3 Main** | `209.244.0.3` | زیرساخت بین‌المللی |
| **`19`** | **AdGuard Public** | `94.140.14.14` / `94.140.15.15` | مسدودسازی تبلیغات |
| **`20`** | **Control D** | `76.76.2.0` / `76.76.10.0` | بدون ثبت لاگ |
| **`21`** | **Comodo Secure** | `8.26.56.26` / `8.20.247.20` | ضد بدافزار |
| **`22`** | **DNS.WATCH** | `84.200.69.80` / `84.200.70.40` | آزادی وب بدون فیلتر |
| **`23`** | **Alternate DNS** | `76.76.19.19` | حذف تبلیغات (در صورت فعال‌بودن سرویس) |
| **`24`** | **Yandex Basic** | `77.88.8.8` / `77.88.8.1` | سرورهای پایه یاندکس |
| **`25`** | **CleanBrowsing (Security)** | `185.228.168.9` / `185.228.169.9` | محافظت در برابر بدافزار و فیشینگ |
| **`26`** | **Pishgaman ISP** | `5.202.100.100` / `5.202.100.101` | شبکه پیشگامان |
| **`27`** | **Shatel ADSL** | `85.15.1.14` / `85.15.1.15` | شبکه ADSL شاتل |

---

### 🛠️ راهنمای شناسایی DNS در گزینه `[V]`
اگر در گزارش تایید، عبارت `Custom User Configured DNS` دیدید و آی‌پی فعال یکی از موارد زیر بود، این‌ها DNSهای رسمی ISPهای ایرانی هستند:

| آی‌پی | سرویس‌دهنده |
| :--- | :--- |
| `5.200.200.200` | مخابرات ایران (TCI) |
| `217.218.127.127` | دیتاسنتر ایران (DCI) |
| `85.15.1.14` / `85.15.1.15` | شاتل |
| `5.202.100.100` / `5.202.100.101` | پیشگامان |

---

### ❓ سوالات متداول (FAQ)

**سوال: آیا با تغییر DNS، آی‌پی من عوض می‌شود؟**
خیر. DNS فقط مانند «دفترچه تلفن» آدرس سایت‌ها را پیدا می‌کند. برای تغییر IP عمومی باید از VPN، Proxy یا Tor استفاده کنید.

**سوال: چرا سایت‌هایی مثل `developer.android.com` با Beshkan باز نمی‌شوند؟**
هر DNS تحریم‌شکن لیست پوششی مخصوص خودش را دارد. Beshkan مخصوص Adobe/Nvidia/Unity/Intel است و سرویس‌های گوگل را پوشش نمی‌دهد. برای توسعه‌دهندگان اندروید از **شکن (Shecan)** استفاده کنید.

**سوال: بهترین ترکیب DNS چیست؟**
DNS Primary را روی یک تحریم‌شکن (مانند شکن) و DNS Secondary را روی یک DNS سریع جهانی (مانند Cloudflare) قرار دهید تا هم تحریم دور زده شود و هم سرعت مرور بالا بماند.

---

## 🇬 English Documentation

### 🌟 About The Project
**1-Click DNS Changer & Anti-Sanction Tester** is an ultra-fast, lightweight Windows utility designed to test ICMP latency and UDP Port 53 DNS responsiveness for **27+ major Iranian Anti-Sanction & Global DNS servers**.

It allows users to switch their network DNS with **a single click (entering option 1-27)** directly on their physical network adapters (Wi-Fi / Ethernet), ignoring virtual VPN adapters (like NordLynx or WireGuard).

---

### ⚠️ Important Notice
> Changing DNS **does NOT change your Public IP**. It only bypasses geo-restrictions applied at the DNS or CDN level (e.g., Adobe, Nvidia, Google Developer, Unity).
> For services that strictly validate your IP address, you still need a proper VPN.

---

### ✨ Key Features
* **⚡ Ultra-Fast Parallel Testing:** Multi-threaded testing of 27+ servers in under 2 seconds.
* **🛡️ Anti-Sanction Labeling:** Explicitly tags Iranian anti-sanction DNS providers (`[Anti-Sanction]`) such as Shecan, Radar Game, Electro, 403 Online, Begzar, etc.
* **🔍 Dual Inspection (Ping + Port 53 Query):** Verifies if DNS port 53 is active or blocked/hijacked by your ISP.
* **🎯 Physical Adapter Filtering:** Automatically targets physical hardware network cards (e.g. Wi-Fi `Realtek RTL8852BE`) while filtering out virtual VPN adapters.
* **🔎 Live System OS Verification `[V]`:** Queries Windows OS directly for applied DNS addresses and performs live domain resolution tests (e.g. `developer.android.com`).
* **🔄 1-Click DHCP Reset `[R]`:** Restores network adapter DNS to automatic DHCP.
* **🧹 Flush DNS Cache `[F]`:** Automatically executes `ipconfig /flushdns`.
* **👑 Self-Elevating Privileges:** Requests Administrator UAC permissions automatically for seamless network configuration.

---

### 🚀 Quick Start & Usage

1. Clone the repository or download **`DNS_Changer.bat`** and **`DNS_Changer.ps1`**:
   ```bash
   git clone https://github.com/Hadooshi/Iran-DNS-Toolbox.git
   ```
2. Double-click **`DNS_Changer.bat`**.
3. Type any option number (**`1`** to **`27`**) and press Enter to instantly apply that DNS!

> 📋 For the complete server list with verified IPs, see the Persian tables above.

---

### 📊 Command Options Summary
| Command | Action | Description |
| :---: | :--- | :--- |
| **`1 - 27`** | **1-Click Apply** | Instantly applies selected DNS to physical Wi-Fi/Ethernet adapter |
| **`V`** | **Verify System DNS** | Queries Windows OS network stack + runs live domain resolution tests |
| **`0`** | **Auto-Set Fastest** | Automatically selects and applies the fastest working DNS from pings |
| **`R`** | **Reset to DHCP** | Restores DNS configuration to automatic router DHCP |
| **`F`** | **Flush Cache** | Runs `ipconfig /flushdns` |
| **`T`** | **Re-Test** | Reruns parallel ping & port 53 reachability tests |
| **`Q`** | **Quit** | Exits the application |

---

### 📄 License
Distributed under the MIT License. See `LICENSE` for more information.

---

<div align="center">
  Made with ❤️ for Iranian developers and gamers
</div>
