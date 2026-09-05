# ⚡ 1-Click DNS Changer & Anti-Sanction Tool (2026)

<div align="center">

[![Windows](https://img.shields.io/badge/OS-Windows%2010%20%2F%2011-blue.svg?style=flat-square&logo=windows)](https://microsoft.com)
[![Version](https://img.shields.io/badge/Version-2.2.1-0078D4.svg?style=flat-square&logo=windows)](https://github.com/Hadooshi/Iran-DNS-Toolbox/releases/latest)
[![Author](https://img.shields.io/badge/Author-Hadooshi-blueviolet.svg?style=flat-square&logo=github)](https://github.com/Hadooshi)
[![.NET](https://img.shields.io/badge/.NET-6.0%20WPF-512BD4.svg?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)
[![Download Latest Release](https://img.shields.io/badge/Download-DNSChanger.exe-success?style=flat-square&logo=windows)](https://github.com/Hadooshi/Iran-DNS-Toolbox/releases/latest)
[![Servers](https://img.shields.io/badge/DNS%20Servers-47%20Verified-informational.svg?style=flat-square)](https://github.com/Hadooshi/Iran-DNS-Toolbox)
[![License](https://img.shields.io/badge/License-MIT-green.svg?style=flat-square)](LICENSE)

**ابزار جامع، فوق‌سریع و هوشمند تغییر DNS و عبور از تحریم‌های اینترنتی برای ویندوز**  
**شامل ۴۷ سرور تست‌شده، مرتب‌سازی هوشمند و راستی‌آزمایی زنده ۲۰ سرویس بین‌المللی**

[![Download Now](https://img.shields.io/badge/%E2%AC%87%EF%B8%8F%20%D8%AF%D8%A7%D9%86%D9%84%D9%88%D8%AF%20%D9%85%D8%B3%D8%AA%D9%82%DB%8C%D9%85%20%D9%86%D8%B1%D9%85%E2%80%8C%D8%A7%D9%81%D8%B2%D8%A7%D8%B1%20(DNSChanger.exe)-2ea44f?style=for-the-badge&logo=windows)](https://github.com/Hadooshi/Iran-DNS-Toolbox/releases/latest)

</div>

---

<div dir="rtl" align="right">

## 🇮🇷 راهنمای فارسی

### 🌟 درباره پروژه
پروژه **DNS Changer & Anti-Sanction Tool** یک نرم‌افزار نیتیو، بسیار سریع، سبک و مدرن برای سیستم‌عامل ویندوز توسط **[Hadooshi](https://github.com/Hadooshi)** است. این ابزار به صورت همزمان وضعیت پینگ واقعی (ICMP) و باز بودن پورت ۵۳ (UDP Query) بیش از **۴۷ سرور DNS** (شامل سرورهای رفع تحریم ایران، ستون فقرات جهانی، حریم خصوصی، خانواده و ISPها) را سنجش کرده و امکان تنظیم ۱-کلیکی آن‌ها را مستقیماً روی **کارت شبکه فیزیکی سیستم (وای‌فای یا اترنت)** فراهم می‌کند.

---

### ⚠️ نکته فنی بسیار مهم
> تغییر دی‌ان‌اس **آدرس آی‌پی عمومی (Public IP) شما را تغییر نمی‌دهد**؛ بلکه تحریم‌ها و محدودیت‌هایی را برطرف می‌کند که در لایه DNS یا شبکه توزیع محتوا (CDN) اعمال شده‌اند (نظیر Adobe, Nvidia, Google Developer, Unity, Spotify و...).  
> برای سرویس‌هایی که مستقیماً کشور مبدا IP را مسدود می‌کنند، در کنار DNS مناسب ممکن است به ابزارهای تغییر IP نیاز داشته باشید.

---

### ✨ قابلیت‌های برجسته نرم‌افزار

* 📁 **تک‌فایل و کاملاً پرتابل (`DNSChanger.exe`):** بدون نیاز به نصب؛ تنها با دابل‌کلیک در هر پوشه‌ای اجرا می‌شود (حجم بسیار سبک کمتر از ۴۰۰ کیلوبایت).
* 👑 **درخواست خودکار مجوز Administrator:** مجهز به مانیفست رسمی ویندوز جهت اعمال بی‌نقص تنظیمات شبکه بدون باز شدن پنجره سیاه کنسول (`cmd.exe`).
* 🎨 **طراحی اصیل و مدرن Fluent Dark** با نمایش بج نسخه `v2.2.1` و دسترسی سریع.
* 🖥️ **سازگاری کامل با فول‌اسکرین و تسک‌بار ویندوز:** عدم پوشاندن تسک‌بار در حالت ماکسیمایز با هوک نیتیو `WM_GETMINMAXINFO` و قابلیت تغییر ابعاد پنجره از تمام لبه‌ها.
* 📐 **گرید واکنش‌گرا و فیت کامل کارت‌ها (`UniformGrid`):** حذف کامل هرگونه فضای خالی در سمت راست و کشیدگی متقارن و یکنواخت کارت‌ها در هر اندازه پنجره یا تمام‌صفحه.
* 🔀 **مرتب‌سازی هوشمند سرورها (`Smart Sorting`):** چینش سریع سرورها با ۵ حالت کاربردی:
  * **⚡ کمترین تاخیر (پینگ):** انتقال سریع‌ترین و پایدارترین سرورها به صدر لیست پس از تست تاخیر.
  * **📶 بیشترین تاخیر (پینگ):** تفکیک سرورهای با تاخیر یا پینگ بالا.
  * **🔤 نام سرور (A-Z):** مرتب‌سازی الفبایی فارسی و انگلیسی.
  * **🛡️ پورت ۵۳ (سالم اول):** اولویت دادن به سرورهایی که پورت ۵۳ آن‌ها باز است و بدون دستکاری پاسخ می‌دهند.
  * **پیش‌فرض:** چیدمان استاندارد بر پایه شناسه و دسته‌بندی موضوعی سرورها.
* 🐙 **دکمه دسترسی مستقیم به گیت‌هاب:** آیکون اختصاصی گیت‌هاب در نوار عنوان جهت مشاهده سورس‌کد، ستاره دادن و پیگیری آپدیت‌ها در مخزن رسمی ([Hadooshi/Iran-DNS-Toolbox](https://github.com/Hadooshi/Iran-DNS-Toolbox)).
* 🎛️ **تغییر دلخواه ابعاد کارت‌ها (اسلایدر):** قابلیت تنظیم روان عرض کارت‌ها از ۱۸۵ تا ۵۱۰ پیکسل، به همراه دکمه‌های چینش سریع:
  * **۵ تایی (فشرده):** جا گرفتن ۵ کارت در هر سطر برای مانیتورهای استاندارد.
  * **۴ تایی و ۳ تایی:** چیدمان متعادل و استاندارد.
  * **۲ تایی (بزرگ):** کارت‌های عریض با جزئیات کامل.
* 📊 **حالت نمایش جدولی/فهرستی (Table View):** سوییچ فوری به جدول افقی بسیار فشرده برای مشاهده تمام مشخصات، تاخیر زنده و اتصال ۱-کلیکی در یک ردیف.
* 🛡️ **راستی‌آزمایی جامع ۲۰ سرویس تحریم:** سنجش تفکیک نام DNS و دسترسی واقعی به وب با تشخیص دقیق خطای تحریم ۴۰۳ (مانند تحریم مستقیم گوگل، جمینای، OpenAI و Claude) به صورت Real-time در هر بار کلیک.
* 🎯 **شناسایی خودکار کارت فیزیکی:** انتخاب خودکار کارت وای‌فای یا اترنت متصل و فیلتر کردن اتوماتیک کارت‌های مجازی VPN (مانند NordLynx, WireGuard, TAP).
* ⚡ **اتصال سریع‌ترین سرور (`Auto Fastest`):** پیدا کردن کمترین پینگ با پورت ۵۳ فعال و ست کردن آن با یک کلیک.
* 🔄 **بازگردانی سریع به خودکار (`Reset DHCP`):** برگرداندن تنظیمات DNS کارت شبکه به حالت خودکار مودم بدون نیاز به ریستارت.
* 🛠️ **تعمیر کامل و اضطراری شبکه (`Emergency Reset`):** اجرای زنجیره کامل فرمان‌های ریست استک شبکه (`ipconfig /flushdns`, `/release`, `/renew`, `netsh winsock reset`, `netsh int ip reset`) برای مواقعی که شبکه ویندوز دچار اختلال اساسی شده است همراه با پیام ریستارت سیستم.
* ➕ **کارت‌های DNS سفارشی دائمی (`Custom DNS`):** تبدیل خودکار هر دی‌ان‌اس دستی به یک کارت مستقل با پینگ زنده، ذخیره‌سازی پرتابل در فایل `custom_dns.json`، تب اختصاصی فیلتر و امکان حذف کارت.
* 👑 **پشتیبانی از شکن حرفه‌ای (Shecan Pro):** افزوده شدن کارت اختصاصی شکن پرو همراه با نشان هشدار و راهنمای ثبت IP جهت جلوگیری از قطعی اتصال.
* 🧹 **پاک‌سازی کش DNS (`Flush DNS`):** اجرای فوری و خودکار دستور `ipconfig /flushdns`.
* 📋 **کپی سریع آی‌پی‌ها:** دکمه کپی اختصاصی برای هر سرور جهت انتقال آدرس‌ها به کلیپ‌بورد.

---

### 🚀 نحوه اجرا

این ابزار به دو شیوه قابل استفاده است:

#### روش اول: نرم‌افزار گرافیکی و پرتابل ویندوز (پیشنهادی ⭐)
1. آخرین نسخه آماده را از **[صفحه انتشار (Releases)](https://github.com/Hadooshi/Iran-DNS-Toolbox/releases/latest)** دانلود کنید.
2. روی فایل **`DNSChanger.exe`** دابل‌کلیک کنید. برنامه بلافاصله باز شده و پس از سنجش پینگ، آماده استفاده است.

#### روش دوم: محیط خط فرمان و ترمینال (CLI)
1. روی فایل **`DNS_Changer.bat`** دابل‌کلیک کنید.
2. پنجره ترمینال باز شده و پینگ سرورها را بررسی می‌کند.
3. عدد سرور مورد نظر (از **`1`** تا **`47`**) را وارد کرده و کلید Enter را بزنید.

---

### 🌐 دسته‌بندی سرورهای DNS (۴۸ سرور)

#### ۱. سرورهای رفع تحریم و کاهش پینگ گیمینگ ایران (Anti-Sanction)
| ردیف | نام سرویس | آی‌پی اصلی (Primary) | آی‌پی ثانویه (Secondary) | کاربرد و کارایی |
| :---: | :--- | :--- | :--- | :--- |
| **۱** | **شکن (Shecan)** | `178.22.122.100` | `185.51.200.2` | وبسایت‌ها، استیم و لانچرهای بازی |
| **۵۰** | **شکن حرفه‌ای (Shecan Pro)** | `178.22.122.101` | `185.51.200.1` | سرور ویژه شکن با پایداری بالاتر (نیازمند اکانت و ثبت IP) |
| **۲** | **رادار گیم (Radar Game)** | `10.202.10.10` | `10.202.10.11` | گیمینگ و کاهش پینگ (Xbox و PC) |
| **۳** | **الکترو (Electro)** | `78.157.42.100` | `78.157.42.101` | مچمیکینگ استیم، پلی‌استیشن و آنلاین |
| **۴** | **بگذر سرور ۱ (Begzar 1)** | `185.55.226.26` | `185.55.225.25` | دانلود بازی و لودینگ وب کامپیوتر |
| **۵** | **بگذر سرور ۲ (Begzar 2)** | `185.55.224.24` | `185.55.225.25` | دانلود بازی و وبگردی سریع |
| **۶** | **۴۰۳ آنلاین (403 Online)** | `10.202.10.202` | `10.202.10.102` | ابزارها و کتابخانه‌های برنامه‌نویسی |
| **۷** | **وانیلا (Vanilla)** | `10.139.177.21` | `10.139.177.22` | هوش مصنوعی، دانلود و بازی‌ها |
| **۸** | **هاست ایران ۱ (HostIran)** | `172.29.0.100` | `172.29.2.100` | وبسایت‌ها و اپلیکیشن‌های خارجی |
| **۹** | **هاست ایران ۲ (HostIran)** | `172.28.2.100` | `179.29.0.100` | خدمات ابری و اپلیکیشن‌ها |
| **۱۰** | **شلتر (Shelter)** | `94.103.125.157` | `94.103.125.158` | پل ارتباطی بازی‌های آنلاین و پینگ |
| **۱۱** | **بشکن (Beshkan)** | `181.41.194.177` | `181.41.194.186` | رفع تحریم Adobe, Nvidia, Unity, Intel |
| **۲۸** | **پارس آنلاین (Pars Online)** | `91.99.101.12` | — | ریزالور ضد تحریم پارس آنلاین |
| **۲۹** | **ابر باران (AbrBaran IDC)** | `172.16.1.100` | `172.16.2.100` | ضد تحریم کلاینت‌های دیتاسنتر |

#### ۲. سرورهای عمومی و ستون فقرات جهانی (Global Public)
| ردیف | نام سرویس | آی‌پی اصلی | آی‌پی ثانویه | توضیحات |
| :---: | :--- | :--- | :--- | :--- |
| **۱۲** | **Cloudflare** | `1.1.1.1` | `1.0.0.1` | سریع‌ترین پاسخگویی و پایداری شبکه |
| **۱۳** | **Google Public** | `8.8.8.8` | `8.8.4.4` | سازگاری حداکثری با تمام ISPها |
| **۱۴** | **Google / Level3** | `4.2.2.4` | `4.2.2.2` | پایداری فوق‌العاده در مسیریابی |
| **۱۵** | **OpenDNS (Cisco)** | `208.67.222.222` | `208.67.220.220` | امنیت سایبری و پالایش شبکه سیسکو |
| **۱۶** | **Quad9** | `9.9.9.9` | `149.112.112.112` | حریم خصوصی و ضد فیشینگ |
| **۱۷** | **NTT Asia** | `129.250.35.250` | `129.250.35.251` | مسیریابی بهینه سرورهای قاره آسیا |
| **۱۸** | **Level3 Main** | `209.244.0.3` | `209.244.0.4` | ستون فقرات زیرساخت بین‌المللی |
| **۱۹** | **AdGuard Public** | `94.140.14.14` | `94.140.15.15` | مسدودسازی تبلیغات و ترکرها |
| **۲۰** | **Control D** | `76.76.2.0` | `76.76.10.0` | سرعت بالا بدون ذخیره لاگ |
| **۲۱** | **Comodo Secure** | `8.26.56.26` | `8.20.247.20` | سپر محافظت در برابر بدافزارها |
| **۲۲** | **DNS.WATCH** | `84.200.69.80` | `84.200.70.40` | بدون سانسور، سریع و بدون فیلتر |
| **۲۳** | **Alternate DNS** | `76.76.19.19` | `76.76.20.20` | حذف تبلیغات مزاحم و تسریع وب |
| **۲۴** | **Yandex Basic** | `77.88.8.8` | `77.88.8.1` | سرورهای پایه و پایدار یاندکس |
| **۲۵** | **CleanBrowsing Security** | `185.228.168.9` | `185.228.169.9` | فیلتر امنیتی ضد مخرب و فیشینگ |
| **۴۴** | **KT Korea** | `168.126.63.1` | `168.126.63.2` | پینگ بهینه بازی‌های آنلاین شرق آسیا |
| **۴۵** | **Hurricane Electric** | `74.82.42.42` | — | پایداری عالی در مسیریابی آمریکا |
| **۴۶** | **Verisign / UltraDNS** | `64.6.64.6` | `64.6.65.6` | حداکثر پایداری بدون ثبت لاگ |
| **۴۷** | **Neustar UltraDNS** | `156.154.70.1` | `156.154.71.1` | زیرساخت پایدار سازمانی بین‌المللی |

#### ۳. سرورهای حریم خصوصی و ضدسانسور (Privacy & No-Log)
| ردیف | نام سرویس | آی‌پی اصلی | آی‌پی ثانویه | توضیحات |
| :---: | :--- | :--- | :--- | :--- |
| **۳۱** | **Mullvad** | `194.242.2.2` | — | بدون لاگ و حداکثر حریم خصوصی |
| **۳۲** | **UncensoredDNS** | `91.239.100.100` | `89.233.43.71` | وب باز و بدون سانسور (دانمارک) |
| **۳۳** | **LibreDNS** | `116.202.176.26` | — | حریم خصوصی و امنیت بدون لاگ (آلمان) |
| **۳۴** | **DNS.SB** | `185.222.222.222` | `45.11.45.11` | ضد سانسور با پشتیبانی از DNSSEC |
| **۳۵** | **AdGuard Non-filtering** | `94.140.14.140` | `94.140.14.141` | ادگارد بدون فیلتر (حداکثر سرعت) |
| **۴۰** | **Quad9 No-Filter** | `9.9.9.10` | `149.112.112.10` | سرور سریع Quad9 بدون فیلترینگ |

#### ۴. سرورهای محافظت خانواده و مسدودکننده بدافزار (Family & Security)
| ردیف | نام سرویس | آی‌پی اصلی | آی‌پی ثانویه | توضیحات |
| :---: | :--- | :--- | :--- | :--- |
| **۳۶** | **Cloudflare Malware** | `1.1.1.2` | `1.0.0.2` | مسدودسازی بدافزار و سایت‌های مخرب |
| **۳۷** | **Cloudflare Family** | `1.1.1.3` | `1.0.0.3` | فیلتر محتوای نامناسب به همراه بدافزار |
| **۳۸** | **OpenDNS FamilyShield** | `208.67.222.123` | `208.67.220.123` | فیلتر محافظت خانواده سیسکو |
| **۳۹** | **AdGuard Family** | `94.140.14.15` | `94.140.15.16` | فیلتر خانوادگی به همراه حذف تبلیغات |
| **۴۱** | **Yandex Safe** | `77.88.8.88` | `77.88.8.2` | فیلتر ضد فیشینگ و مخرب یاندکس |
| **۴۲** | **Yandex Family** | `77.88.8.7` | `77.88.8.3` | فیلتر محافظت خانواده یاندکس |
| **۴۳** | **CleanBrowsing Family** | `185.228.168.168` | `185.228.169.168` | فیلتر جامع و امنیتی خانواده |

#### ۵. سرورهای داخلی تامین‌کنندگان اینترنت (Domestic ISP)
| ردیف | نام سرویس | آی‌پی اصلی | آی‌پی ثانویه | توضیحات |
| :---: | :--- | :--- | :--- | :--- |
| **۲۶** | **پیشگامان (Pishgaman)** | `5.202.100.100` | `5.202.100.101` | شبکه پیشگامان ADSL و فیبرنوری |
| **۲۷** | **شاتل (Shatel ADSL)** | `85.15.1.14` | `85.15.1.15` | اینترنت و شبکه ADSL شاتل |
| **۳۰** | **آسیاتک (AsiaTech)** | `194.36.174.161` | `178.22.122.100` | مشترکین آسیاتک و پینگ پایین |

---

### 🔍 ۲۰ سرویس کلیدی در بخش راستی‌آزمایی (Verification)
در پنجره راستی‌آزمایی برنامه، وضعیت اتصال و حل دامنه سرویس‌های زیر به طور زنده ارزیابی می‌شود:

* **هوش مصنوعی (AI):** Antigravity (Google), ChatGPT (OpenAI), Claude (Anthropic), Gemini (Google), Perplexity
* **توسعه و برنامه‌نویسی (Dev):** Google, Google Android Developers, GitHub
* **طراحی و گرافیک (Creative):** Adobe, Nvidia
* **رسانه و صوت (Media):** Spotify, Spotify for Creators
* **بازی و سرگرمی (Gaming):** Epic Games Store, Steam
* **آموزش آنلاین (Learning):** Mimo, Duolingo, Coursera
* **فریلنسری بین‌المللی (Freelance):** Upwork, Fiverr, Freelancer

---

### 🛠️ نحوه بیلد مجدد سورس‌کد
اگر تغییری در سورس‌کد دادید، تنها با اجرای فایل زیر در مسیر پروژه، فایل پرتابل مجدداً کامپایل می‌شود:
```cmd
build.bat
```

</div>

---

<div dir="ltr" align="left">

## 🇬 English Documentation

### 🌟 About The Project
**DNS Changer & Anti-Sanction Tool** (v2.2.0) is a native, ultra-fast, lightweight Windows desktop application by **[Hadooshi](https://github.com/Hadooshi)** designed to test ICMP latency and UDP Port 53 reachability for **47+ major Anti-Sanction & Global DNS servers**.

It allows users to switch network DNS with **1 click** directly on their physical network adapter (Wi-Fi or Ethernet), while automatically ignoring virtual VPN adapters (such as NordLynx, WireGuard, and TAP interfaces).

---

### ✨ Features
* **Portable Single-File Executable (`DNSChanger.exe`):** No installation needed; ultra-compact (~360 KB).
* **Native Windows 11 Fluent Dark UI:** Modern, clean dark theme with titlebar version badge and authentic vector glyphs.
* **Smart Multi-Criteria Sorting:** Sort DNS servers and table rows instantly by:
  * **⚡ Lowest Latency (Fastest Ping)**
  * **📶 Highest Latency**
  * **🔤 Server Name (A-Z Alphabetical)**
  * **🛡️ Port 53 Status (Active & Working First)**
  * **Default (Category & ID order)**
* **Direct GitHub Integration:** Title bar vector button opening the official repository ([Hadooshi/Iran-DNS-Toolbox](https://github.com/Hadooshi/Iran-DNS-Toolbox)).
* **Flexible Layout & Card Size Slider:** Dynamic card resizing (185px to 510px) with 5-card, 4-card, 3-card, and 2-card per row presets.
* **Compact Table/List View:** Highly dense horizontal table view for power users.
* **Comprehensive 20-Service Sanction Verification:** Real-time DNS resolution and HTTP geo-block detection (code 403) for AI, Dev, Creative, Gaming, Media, Learning, and Freelance services.
* **Multi-threaded Testing:** Parallel ping and UDP DNS query testing for all 47 servers in under 2 seconds.
* **1-Click Actions:** Auto-set fastest DNS, reset to DHCP, flush DNS cache (`ipconfig /flushdns`), emergency network stack reset, and persistent custom DNS cards.
* **Self-Elevating Admin:** Embedded Windows application manifest for silent UAC elevation.

---

### 🚀 Usage
1. Download **`DNSChanger.exe`** from **[GitHub Releases](https://github.com/Hadooshi/Iran-DNS-Toolbox/releases/latest)**.
2. Run **`DNSChanger.exe`** directly by double-clicking it.
3. Select your physical network adapter (auto-detected).
4. Click **Connect** on any desired DNS card or table row, or click **Auto Fastest**.

---

### 📄 License
Distributed under the MIT License. See [LICENSE](LICENSE) for more details.

</div>
