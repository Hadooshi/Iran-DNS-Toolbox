# ⚡ 1-Click DNS Changer & Anti-Sanction Tester (2026)

[![Windows](https://img.shields.io/badge/OS-Windows%2010%20%2F%2011-blue.svg)](https://microsoft.com)
[![PowerShell](https://img.shields.io/badge/PowerShell-5.1%2B-blue.svg)](https://microsoft.com/powershell)
[![DNS Servers](https://img.shields.io/badge/DNS%20Servers-47-orange.svg)](#-جدول-سرورهای-dns-پشتیبانیشده)
[![Service Tests](https://img.shields.io/badge/Service%20Tests-20-purple.svg)](#-تست-دسترسی-به-سرویسهای-تحریمشده-گزینه-s)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Status](https://img.shields.io/badge/Status-Active-brightgreen.svg)](https://github.com/Hadooshi/Iran-DNS-Toolbox)

---

## 🇮🇷 راهنمای فارسی (Persian Documentation)

### 🌟 درباره پروژه

&rlm;ابزار **1-Click DNS Changer & Anti-Sanction Tester** یک برنامه هوشمند، قدرتمند و فوق‌العاده سریع برای سیستم‌عامل ویندوز است. این ابزار به‌صورت همزمان **۴۷ سرور DNS** ایرانی (رفع تحریم و کاهش پینگ گیمینگ) و بین‌المللی را تست کرده و پینگ واقعی، پاسخگویی پورت ۵۳ و پشتیبانی از **DoH روی پورت ۴۴۳** را می‌سنجد.

&rlm;با این برنامه می‌توانید تنها با **یک کلیک (وارد کردن عدد ۱ تا ۴۷)**، DNS دلخواه خود را مستقیماً روی **کارت شبکه فیزیکی سیستم (Wi-Fi یا اترنت)** اعمال کنید و کارت‌های مجازی VPN (مانند NordLynx) را نادیده بگیرید.

&rlm;همچنین با گزینه‌های `[V]` و `[S]` برنامه **۲۰ سرویس تحریم‌شده** (چت جی‌پی‌تی، کلاد، جمنای، انتی‌گراویتی، ادوبی، انویدیا، استیم، اپیک گیمز، گیت‌هاب، اسپاتیفای و…) را با DNS فعلی شما تست می‌کند و برای هرکدام **تایید (PASS) یا رد (FAIL)** می‌دهد.

---

### ⚠️ توجه بسیار مهم (حتماً بخوانید)

> &rlm;تغییر DNS **آی‌پی عمومی (Public IP) شما را عوض نمی‌کند**؛ بلکه فقط تحریم‌هایی را دور می‌زند که بر اساس **DNS یا لوکیشن CDN** اعمال می‌شوند (مانند Adobe، Nvidia، Google Developer، Unity و…).
>
> &rlm;اگر سرویسی دقیقاً IP شما را بررسی می‌کند، DNS به‌تنهایی کافی نیست و به VPN نیاز دارید. در گزارش تست سرویس‌ها، این حالت با برچسب **PARTIAL** نمایش داده می‌شود.

---

### ✨ ویژگی‌های اصلی

* **⚡ تست موازی فوق‌العاده سریع:** &rlm;سنجش هر ۴۷ سرور تنها در چند ثانیه با Parallel Runspace.
* **🔐 ستون اختصاصی DoH:** &rlm;برای هر سرور مشخص می‌شود که آیا **DNS-over-HTTPS روی پورت ۴۴۳** دارد و آیا آن endpoint از شبکه شما در دسترس است یا خیر.
* **🧪 تست ۲۰ سرویس تحریم‌شده:** &rlm;بررسی زنده ChatGPT، Claude، Gemini، Antigravity، Perplexity، Adobe، Nvidia، Spotify، Epic Games، Steam، Mimo، Duolingo، GitHub، Coursera، Upwork، Fiverr، Freelancer و… با نتیجه **PASS / PARTIAL / FAIL**.
* **🛡️ برچسب‌گذاری اختصاصی تحریم‌شکن‌ها:** &rlm;مشخص کردن سرورهای رفع تحریم ایران با تگ `[Anti-Sanction]` نظیر شکن، رادار گیم، الکترو، ۴۰۳ آنلاین، بگذر، پارس آنلاین و…
* **🔍 تست دوگانه (پینگ ICMP + پورت ۵۳ DNS):** &rlm;سنجش واقعی اینکه آیا ISP شما DNS را فیلتر یا هایجک کرده است یا خیر.
* **🎯 هدف‌گیری دقیق کارت شبکه فیزیکی:** &rlm;شناسایی کارت شبکه اصلی (مانند وای‌فای `Realtek RTL8852BE`) و نادیده گرفتن تونل‌های VPN.
* **🚫 تشخیص سرورهای DoH-Only:** &rlm;سرویس‌هایی مثل Mullvad که روی UDP/53 جواب نمی‌دهند، علامت‌گذاری شده و قبل از اعمال به شما هشدار داده می‌شود.
* **🔄 بازگردانی سریع به حالت خودکار `[R]`:** &rlm;تنظیم مجدد DNS روی حالت پیش‌فرض DHCP.
* **🧹 پاک‌سازی کش سیستم `[F]`:** &rlm;اجرای اتوماتیک `ipconfig /flushdns`.
* **👑 ارتقای اتوماتیک دسترسی Administrator:** &rlm;درخواست خودکار UAC جهت اعمال بدون خطای تغییرات شبکه.

---

### 🚀 نحوه نصب و اجرا

&rlm;۱. مخزن را کلون کنید یا فایل‌های `DNS_Changer.bat` و `DNS_Changer.ps1` را دانلود کنید:

```bash
git clone https://github.com/Hadooshi/Iran-DNS-Toolbox.git
```

&rlm;۲. روی فایل `DNS_Changer.bat` دابل‌کلیک کنید (دسترسی ادمین به‌صورت خودکار درخواست می‌شود).

&rlm;۳. در پنجره بازشده، شماره DNS مورد نظر (از `1` تا `47`) را تایپ کرده و Enter بزنید.

&rlm;۴. برای گرفتن گزارش تایید/رد سرویس‌های تحریمی، کلید `S` را بزنید.

---

### 📊 منوی دستورات سریع

| کلید | دستور | عملکرد |
| :---: | :--- | :--- |
| **`1 - 47`** | تغییر ۱-کلیکی | &rlm;اعمال آنی DNS انتخابی روی کارت شبکه فیزیکی وای‌فای/اترنت |
| **`V`** | تایید وضعیت سیستم | &rlm;بررسی مستقیم از ویندوز + تست کامل ۲۰ سرویس تحریم‌شده |
| **`S`** | تست سرویس‌ها | &rlm;فقط اجرای تست دسترسی ۲۰ سرویس تحریمی (سریع‌تر از `V`) |
| **`D`** | فهرست DoH | &rlm;نمایش تمام endpointهای DNS-over-HTTPS روی پورت ۴۴۳ |
| **`0`** | تنظیم خودکار | &rlm;ست کردن اتوماتیک سریع‌ترین DNS پیدا شده در تست پینگ |
| **`R`** | ریست DNS | &rlm;بازگرداندن تنظیمات شبکه به حالت خودکار (DHCP) |
| **`F`** | پاک‌سازی کش | &rlm;اجرای دستور `ipconfig /flushdns` |
| **`T`** | تست مجدد | &rlm;تکرار تست پینگ، پورت ۵۳ و DoH برای تمام سرورها |
| **`Q`** | خروج | &rlm;بستن برنامه |

---

### 🧪 تست دسترسی به سرویس‌های تحریم‌شده (گزینه `S`)

&rlm;برنامه دیگر فقط `google.com` و `developer.android.com` را چک نمی‌کند. با هر بار اعمال DNS و همچنین با فشردن `V` یا `S`، این ۲۰ سرویس به‌صورت موازی تست می‌شوند (ابتدا **حل نام دامنه** و سپس **دست‌دادن HTTPS**):

| گروه | سرویس | دامنه تست‌شده |
| :--- | :--- | :--- |
| هوش مصنوعی | Antigravity | `antigravity.google` |
| هوش مصنوعی | ChatGPT | `chatgpt.com` |
| هوش مصنوعی | Claude | `claude.ai` |
| هوش مصنوعی | Gemini | `gemini.google.com` |
| هوش مصنوعی | Perplexity | `www.perplexity.ai` |
| توسعه | Google | `www.google.com` |
| توسعه | Google Android Dev | `developer.android.com` |
| توسعه | GitHub | `github.com` |
| خلاقیت | Adobe | `www.adobe.com` |
| خلاقیت | Nvidia | `www.nvidia.com` |
| رسانه | Spotify | `open.spotify.com` |
| رسانه | Spotify for Creators | `creators.spotify.com` |
| گیمینگ | Epic Games Store | `store.epicgames.com` |
| گیمینگ | Steam | `store.steampowered.com` |
| آموزش | Mimo | `mimo.org` |
| آموزش | Duolingo | `www.duolingo.com` |
| آموزش | Coursera | `www.coursera.org` |
| فریلنسری | Upwork | `www.upwork.com` |
| فریلنسری | Fiverr | `www.fiverr.com` |
| فریلنسری | Freelancer | `www.freelancer.com` |

#### 📗 معنی نتایج

| نتیجه | معنی | راهکار |
| :---: | :--- | :--- |
| 🟢 **PASS** | &rlm;هم دامنه حل شد و هم اتصال HTTPS برقرار شد ← تحریم دور زده شد. | — |
| 🟡 **PARTIAL** | &rlm;دامنه حل شد ولی خود سرویس اتصال را رد کرد (معمولاً خطای `403` یا `451`). | &rlm;تحریم در سطح IP است؛ DNS کافی نیست و VPN لازم دارید. |
| 🔴 **FAIL** | &rlm;DNS فعلی اصلاً نتوانست دامنه را حل کند. | &rlm;یک DNS تحریم‌شکن دیگر (شکن، ۴۰۳، بگذر) را امتحان کنید. |

> &rlm;برنامه پیش از هر تست، کش DNS ویندوز را خالی می‌کند تا نتیجه دقیقاً مربوط به DNS تازه‌اعمال‌شده باشد. همچنین پاسخ‌های سینک‌هول (مانند `0.0.0.0` و `127.x`) به‌عنوان بلاک‌شده شناسایی می‌شوند.

---

### 📋 جدول سرورهای DNS پشتیبانی‌شده

#### 🟢 سرورهای رفع تحریم ایران (Anti-Sanction)

| `#` | نام سرویس | آی‌پی Primary | آی‌پی Secondary | DoH | کاربری اصلی |
| :---: | :--- | :--- | :--- | :---: | :--- |
| **`1`** | &rlm;شکن (Shecan) | `178.22.122.100` | `185.51.200.2` | ✅ | &rlm;رفع تحریم عمومی وبسایت‌ها و لانچرها |
| **`2`** | &rlm;رادار گیم (Radar Game) | `10.202.10.10` | `10.202.10.11` | ❌ | &rlm;کاهش پینگ و رفع اختلال بازی‌ها (Xbox/PC) |
| **`3`** | &rlm;الکترو (Electro) | `78.157.42.100` | `78.157.42.101` | ❌ | &rlm;مچ‌میکینگ استیم، پلی‌استیشن و بازی‌های آنلاین |
| **`4`** | &rlm;بگذر سرور ۱ (Begzar 1) | `185.55.226.26` | `185.55.225.25` | ❌ | &rlm;دانلود بازی و لودینگ وب PC |
| **`5`** | &rlm;بگذر سرور ۲ (Begzar 2) | `185.55.224.24` | `185.55.225.25` | ❌ | &rlm;سرور جایگزین بگذر برای دانلود بازی |
| **`6`** | &rlm;۴۰۳ آنلاین (403 Online) | `10.202.10.202` | `10.202.10.102` | ✅ | &rlm;رفع تحریم ابزارها و کتابخانه‌های برنامه‌نویسی |
| **`7`** | &rlm;وانیلا (Vanilla) | `10.139.177.21` | `10.139.177.22` | ❌ | &rlm;سرویس‌های هوش مصنوعی، دانلود و بازی آنلاین |
| **`8`** | &rlm;هاست ایران ۱ (HostIran 1) | `172.29.0.100` | `172.29.2.100` | ❌ | &rlm;آی‌پی‌های رسمی و عمومی آنتی‌تحریم هاست ایران |
| **`9`** | &rlm;هاست ایران ۲ (HostIran 2) | `172.28.2.100` | `179.29.0.100` | ❌ | &rlm;رنج جایگزین، مخصوص سرورهای ابری هاست ایران |
| **`10`** | &rlm;شلتر (Shelter) | `94.103.125.157` | `94.103.125.158` | ❌ | &rlm;پل ارتباطی بازی‌های آنلاین و کاهش پینگ |
| **`11`** | &rlm;بشکن (Beshkan) | `181.41.194.177` | `181.41.194.186` | ❌ | &rlm;رفع تحریم Adobe, Nvidia, Unity, Intel |
| **`28`** | &rlm;پارس آنلاین (Pars Online) 🆕 | `91.99.101.12` | — | ❌ | &rlm;رزالور رایگان آنتی‌تحریم با پینگ داخلی پایین |
| **`29`** | &rlm;ابر باران (AbrBaran IDC) 🆕 | `172.16.1.100` | `172.16.2.100` | ❌ | &rlm;آنتی‌تحریم — رنج داخلی، مخصوص مشتریان دیتاسنتر |
| **`30`** | &rlm;آسیاتک (AsiaTech) 🆕 | `194.36.174.161` | `178.22.122.100` | ❌ | &rlm;مشترکین آسیاتک، پینگ پایین داخلی |

> &rlm;**نکته درباره `9` و `29`:** &lrm;`172.16.x.x`&rlm; و &lrm;`172.28/172.29`&rlm; از رنج‌های خصوصی (RFC 1918) هستند و فقط از داخل شبکه ایران (یا شبکه همان دیتاسنتر) پاسخ می‌دهند. اگر پاسخ نگرفتید، این یک خطای برنامه نیست؛ سرویس روی ISP شما در دسترس نیست.
>
> &rlm;**نکته درباره میهن‌وب‌هاست:** &rlm;این سرویس آنتی‌تحریم را فقط به مشتریان سرویس‌های میزبانی خود و از طریق پنل کاربری ارائه می‌دهد و آی‌پی عمومی و ثابتی منتشر نکرده است؛ به همین دلیل در فهرست برنامه گنجانده نشد. در صورت انتشار آی‌پی عمومی، اضافه خواهد شد.

#### 🌐 سرورهای بین‌المللی (Global)

| `#` | نام سرویس | آی‌پی Primary | آی‌پی Secondary | DoH | توضیحات |
| :---: | :--- | :--- | :--- | :---: | :--- |
| **`12`** | Cloudflare | `1.1.1.1` | `1.0.0.1` | ✅ | &rlm;سریع‌ترین پاسخگویی جهانی |
| **`13`** | Google Public DNS | `8.8.8.8` | `8.8.4.4` | ✅ | &rlm;پایداری و سازگاری حداکثری |
| **`14`** | Level3 (Lumen) | `4.2.2.4` | `4.2.2.2` | ❌ | &rlm;پایداری مسیریابی بین‌المللی |
| **`15`** | OpenDNS (Cisco) | `208.67.222.222` | `208.67.220.220` | ✅ | &rlm;امنیت شبکه و وب‌گردی |
| **`16`** | Quad9 | `9.9.9.9` | `149.112.112.112` | ✅ | &rlm;حریم خصوصی و ضد فیشینگ |
| **`17`** | NTT Asia | `129.250.35.250` | `129.250.35.251` | ❌ | &rlm;مسیریابی بهینه سرورهای آسیا |
| **`18`** | Level3 Main | `209.244.0.3` | `209.244.0.4` | ❌ | &rlm;زیرساخت بین‌المللی |
| **`19`** | AdGuard Public | `94.140.14.14` | `94.140.15.15` | ✅ | &rlm;مسدودسازی تبلیغات و ترکرها |
| **`20`** | Control D | `76.76.2.0` | `76.76.10.0` | ✅ | &rlm;بدون ثبت لاگ، سرعت بالا |
| **`21`** | Comodo Secure | `8.26.56.26` | `8.20.247.20` | ❌ | &rlm;سپر ضد بدافزار |
| **`22`** | DNS.WATCH | `84.200.69.80` | `84.200.70.40` | ❌ | &rlm;آزادی وب بدون فیلتر |
| **`23`** | Alternate DNS | `76.76.19.19` | `76.76.20.20` | ❌ | &rlm;حذف تبلیغات |
| **`24`** | Yandex Basic | `77.88.8.8` | `77.88.8.1` | ❌ | &rlm;سرورهای پایه و بدون فیلتر یاندکس |
| **`25`** | CleanBrowsing Security | `185.228.168.9` | `185.228.169.9` | ✅ | &rlm;محافظت در برابر بدافزار و فیشینگ |
| **`26`** | Pishgaman ISP | `5.202.100.100` | `5.202.100.101` | ❌ | &rlm;شبکه پیشگامان |
| **`27`** | Shatel ADSL | `85.15.1.14` | `85.15.1.15` | ❌ | &rlm;شبکه ADSL شاتل |
| **`45`** | Hurricane Electric 🆕 | `74.82.42.42` | — | ❌ | &rlm;مسیریابی پایدار آمریکا |
| **`46`** | Verisign / UltraDNS 🆕 | `64.6.64.6` | `64.6.65.6` | ❌ | &rlm;پایداری حداکثری، بدون لاگ (اکنون زیرمجموعه Vercara) |
| **`47`** | Neustar UltraDNS 🆕 | `156.154.70.1` | `156.154.71.1` | ❌ | &rlm;زیرساخت سازمانی |

#### 🔐 حریم خصوصی و ضد سانسور (Privacy) 🆕

| `#` | نام سرویس | آی‌پی Primary | آی‌پی Secondary | DoH | توضیحات |
| :---: | :--- | :--- | :--- | :---: | :--- |
| **`31`** | Mullvad ⚠️ | `194.242.2.2` | — | ✅ | &rlm;بدون لاگ — **فقط DoH/DoT؛ روی UDP/53 پاسخ نمی‌دهد** |
| **`32`** | UncensoredDNS | `91.239.100.100` | `89.233.43.71` | ✅ | &rlm;وب آزاد و بدون فیلتر (دانمارک) |
| **`33`** | LibreDNS | `116.202.176.26` | — | ✅ | &rlm;حریم خصوصی، بدون لاگ (آلمان) |
| **`34`** | DNS.SB | `185.222.222.222` | `45.11.45.11` | ✅ | &rlm;ضد سانسور، پشتیبانی کامل DNSSEC |
| **`35`** | AdGuard Non-filtering | `94.140.14.140` | `94.140.14.141` | ✅ | &rlm;AdGuard بدون هیچ فیلتری (فقط سرعت) |
| **`40`** | Quad9 No-Filter | `9.9.9.10` | `149.112.112.10` | ✅ | &rlm;Quad9 بدون بلاکینگ (فقط سرعت) |

> &rlm;⚠️ **مهم درباره Mullvad:** &rlm;این سرویس از سال ۲۰۲۲ DNS رمزنگاری‌نشده خود را تعطیل کرده است. آی‌پی &lrm;`194.242.2.2`&rlm; فقط روی **DoH (پورت ۴۴۳)** و **DoT (پورت ۸۵۳)** کار می‌کند و اگر آن را به‌عنوان DNS معمولی ویندوز ست کنید اینترنت شما قطع می‌شود. برنامه پیش از اعمال، این هشدار را نمایش می‌دهد و تایید می‌گیرد.

#### 🛡️ امنیتی و خانوادگی (Security / Family) 🆕

| `#` | نام سرویس | آی‌پی Primary | آی‌پی Secondary | DoH | کاربرد |
| :---: | :--- | :--- | :--- | :---: | :--- |
| **`36`** | Cloudflare Malware | `1.1.1.2` | `1.0.0.2` | ✅ | &rlm;ضد بدافزار |
| **`37`** | Cloudflare Family | `1.1.1.3` | `1.0.0.3` | ✅ | &rlm;خانوادگی (محتوای بزرگسال + بدافزار) |
| **`38`** | OpenDNS FamilyShield | `208.67.222.123` | `208.67.220.123` | ✅ | &rlm;فیلتر خانوادگی سیسکو |
| **`39`** | AdGuard Family | `94.140.14.15` | `94.140.15.16` | ✅ | &rlm;خانوادگی + ضد تبلیغ |
| **`41`** | Yandex Safe | `77.88.8.88` | `77.88.8.2` | ❌ | &rlm;ضد فیشینگ و بدافزار |
| **`42`** | Yandex Family | `77.88.8.7` | `77.88.8.3` | ❌ | &rlm;فیلتر خانوادگی یاندکس |
| **`43`** | CleanBrowsing Family | `185.228.168.168` | `185.228.169.168` | ✅ | &rlm;محافظت کامل خانواده (اصلاح‌شده) |

> &rlm;**اصلاح آی‌پی:** &rlm;در پیشنهاد اولیه برای CleanBrowsing Family آی‌پی‌های &lrm;`185.228.168.168 / 185.228.169.169`&rlm; ذکر شده بود؛ آی‌پی ثانویه رسمی و درست &lrm;`185.228.169.168`&rlm; است. همچنین برای AdGuard Family جفت رسمی &lrm;`94.140.14.15 / 94.140.15.16`&rlm; تایید شد.

#### 🎮 گیمینگ و مسیریابی منطقه‌ای (Gaming) 🆕

| `#` | نام سرویس | آی‌پی Primary | آی‌پی Secondary | DoH | کاربرد |
| :---: | :--- | :--- | :--- | :---: | :--- |
| **`44`** | KT Korea | `168.126.63.1` | `168.126.63.2` | ❌ | &rlm;پینگ بهتر سرورهای کره و آسیا (بازی‌های آسیایی) |

---

### 🔐 فهرست کامل endpointهای DoH (گزینه `D`)

&rlm;هر سروری که ستون DoH آن ✅ است، روی **پورت ۴۴۳** هم پاسخ می‌دهد. این یعنی حتی اگر ISP شما پورت ۵۳ را هایجک یا فیلتر کرده باشد، همچنان می‌توانید از آن سرویس استفاده کنید.

| سرویس | endpoint (RFC 8484) |
| :--- | :--- |
| Shecan | `https://free.shecan.ir/dns-query` |
| 403 Online | `https://dns.403.online/dns-query` |
| Cloudflare | `https://cloudflare-dns.com/dns-query` |
| Cloudflare Malware | `https://security.cloudflare-dns.com/dns-query` |
| Cloudflare Family | `https://family.cloudflare-dns.com/dns-query` |
| Google | `https://dns.google/dns-query` |
| OpenDNS | `https://doh.opendns.com/dns-query` |
| OpenDNS FamilyShield | `https://doh.familyshield.opendns.com/dns-query` |
| Quad9 | `https://dns.quad9.net/dns-query` |
| Quad9 No-Filter | `https://dns10.quad9.net/dns-query` |
| AdGuard Public | `https://dns.adguard-dns.com/dns-query` |
| AdGuard Family | `https://family.adguard-dns.com/dns-query` |
| AdGuard Non-filtering | `https://unfiltered.adguard-dns.com/dns-query` |
| Control D | `https://freedns.controld.com/p0` |
| CleanBrowsing Security | `https://doh.cleanbrowsing.org/doh/security-filter/` |
| CleanBrowsing Family | `https://doh.cleanbrowsing.org/doh/family-filter/` |
| Mullvad | `https://dns.mullvad.net/dns-query` |
| UncensoredDNS | `https://anycast.uncensoreddns.org/dns-query` |
| LibreDNS | `https://doh.libredns.gr/dns-query` |
| DNS.SB | `https://doh.dns.sb/dns-query` |

#### ⚙️ نحوه فعال‌سازی DoH در ویندوز ۱۱

&rlm;۱. مسیر `Settings → Network & Internet → Wi-Fi (یا Ethernet) → Hardware properties` را باز کنید.

&rlm;۲. کنار `DNS server assignment` روی **Edit** بزنید و حالت را روی **Manual** بگذارید.

&rlm;۳. IPv4 را روشن کرده و آی‌پی‌های سرویس را وارد کنید.

&rlm;۴. گزینه `Preferred DNS encryption` را روی **Encrypted only (DNS over HTTPS)** بگذارید؛ اگر سرویس در فهرست خودکار ویندوز نبود، حالت **On (manual template)** را انتخاب و آدرس بالا را وارد کنید.

&rlm;۵. در فایرفاکس: `Settings → Privacy & Security → DNS over HTTPS → Max Protection → Custom`.

---

### 🛠️ راهنمای شناسایی DNS در گزینه `[V]`

&rlm;اگر در گزارش تایید عبارت `Custom User Configured DNS` را دیدید و آی‌پی فعال یکی از موارد زیر بود، این‌ها DNSهای رسمی ISPهای ایرانی هستند:

| آی‌پی | سرویس‌دهنده |
| :--- | :--- |
| `5.200.200.200` | &rlm;مخابرات ایران (TCI) |
| `217.218.127.127` / `217.218.155.155` | &rlm;مخابرات / دیتاسنتر ایران (DCI) |
| `85.15.1.14` / `85.15.1.15` | &rlm;شاتل |
| `5.202.100.100` / `5.202.100.101` | &rlm;پیشگامان |
| `194.36.174.161` | &rlm;آسیاتک |
| `91.99.101.12` | &rlm;پارس آنلاین |
| `46.224.1.42` | &rlm;داده گستر عصر نوین |

---

### ❓ سوالات متداول (FAQ)

**&rlm;سوال: آیا با تغییر DNS، آی‌پی من عوض می‌شود؟**

&rlm;خیر. DNS فقط مانند «دفترچه تلفن» آدرس سایت‌ها را پیدا می‌کند. برای تغییر IP عمومی باید از VPN، Proxy یا Tor استفاده کنید.

**&rlm;سوال: چرا در تست سرویس‌ها نتیجه PARTIAL می‌گیرم؟**

&rlm;یعنی DNS شما دامنه را درست حل کرده ولی خود سرویس با دیدن آی‌پی ایران، اتصال را رد کرده است (خطای `403` یا `451`). این تحریم در سطح IP است و با DNS قابل رفع نیست.

**&rlm;سوال: چرا سایت‌هایی مثل `developer.android.com` با Beshkan باز نمی‌شوند؟**

&rlm;هر DNS تحریم‌شکن لیست پوششی مخصوص خودش را دارد. Beshkan مخصوص Adobe/Nvidia/Unity/Intel است و سرویس‌های گوگل را پوشش نمی‌دهد. برای توسعه‌دهندگان اندروید از **شکن (Shecan)** یا **۴۰۳ آنلاین** استفاده کنید.

**&rlm;سوال: چند سرور در تست، ستون DoH را `YES [X]` نشان می‌دهند. یعنی چه؟**

&rlm;یعنی آن سرویس واقعاً DoH دارد، ولی endpoint پورت ۴۴۳ آن از شبکه فعلی شما در دسترس نیست (فیلترینگ SNI یا مسدودسازی دامنه). سرویس دیگری را امتحان کنید.

**&rlm;سوال: بهترین ترکیب DNS چیست؟**

&rlm;DNS Primary را روی یک تحریم‌شکن (مانند شکن یا ۴۰۳) و DNS Secondary را روی یک DNS سریع جهانی (مانند Cloudflare) قرار دهید تا هم تحریم دور زده شود و هم سرعت مرور بالا بماند.

---

## 🇬🇧 English Documentation

### 🌟 About The Project

**1-Click DNS Changer & Anti-Sanction Tester** is an ultra-fast, lightweight Windows utility that tests ICMP latency, UDP port 53 responsiveness and **DNS-over-HTTPS (port 443)** availability for **47 Iranian anti-sanction and global DNS servers**.

It switches your network DNS with **a single keystroke (option 1-47)** directly on your physical network adapters (Wi-Fi / Ethernet), ignoring virtual VPN adapters such as NordLynx or WireGuard, and then verifies access to **20 sanctioned services** with a clear PASS / PARTIAL / FAIL verdict for each one.

---

### ⚠️ Important Notice

> Changing DNS **does NOT change your public IP**. It only bypasses geo-restrictions applied at the DNS or CDN level (e.g. Adobe, Nvidia, Google Developer, Unity).
> For services that strictly validate your IP address you still need a proper VPN — those show up as **PARTIAL** in the service report.

---

### ✨ Key Features

* **⚡ Ultra-fast parallel testing** — all 47 servers probed concurrently via runspace pools.
* **🔐 Dedicated DoH column** — live RFC 8484 query against each provider's HTTPS endpoint so you know which servers survive an ISP that hijacks port 53.
* **🧪 20-service sanction report** — ChatGPT, Claude, Gemini, Antigravity, Perplexity, Adobe, Nvidia, Spotify (+ Spotify for Creators), Epic Games, Steam, Mimo, Duolingo, GitHub, Coursera, Upwork, Fiverr, Freelancer, Google and Android Developers.
* **🛡️ Anti-sanction labelling** — Iranian providers explicitly tagged `[Anti-Sanction]`.
* **🔍 Dual inspection** — ICMP ping plus a real UDP/53 DNS query to detect ISP hijacking.
* **🎯 Physical adapter filtering** — targets real hardware NICs, skips VPN tunnels.
* **🚫 DoH-only detection** — providers such as Mullvad that no longer answer on UDP/53 are flagged and require confirmation before being applied.
* **🔄 1-click DHCP reset `[R]`**, **🧹 flush cache `[F]`**, **👑 self-elevating UAC**.

---

### 🚀 Quick Start & Usage

1. Clone the repository or download `DNS_Changer.bat` and `DNS_Changer.ps1`:
   ```bash
   git clone https://github.com/Hadooshi/Iran-DNS-Toolbox.git
   ```
2. Double-click `DNS_Changer.bat` (administrator rights are requested automatically).
3. Type any option number (`1` to `47`) and press Enter to instantly apply that DNS.
4. Press `S` at any time for the sanctioned-service PASS/FAIL report.

> 📋 For the complete server list with verified IPs and DoH endpoints, see the Persian tables above.

---

### 📊 Command Options Summary

| Command | Action | Description |
| :---: | :--- | :--- |
| **`1 - 47`** | 1-Click Apply | Instantly applies the selected DNS to the physical Wi-Fi/Ethernet adapter |
| **`V`** | Verify System DNS | Queries the Windows network stack, then runs the full 20-service test |
| **`S`** | Service Test | Runs only the sanctioned-service accessibility report |
| **`D`** | DoH List | Prints every DNS-over-HTTPS endpoint (port 443) |
| **`0`** | Auto-Set Fastest | Applies the fastest working DNS from the last test |
| **`R`** | Reset to DHCP | Restores automatic router DHCP DNS |
| **`F`** | Flush Cache | Runs `ipconfig /flushdns` |
| **`T`** | Re-Test | Reruns ping, port 53 and DoH reachability tests |
| **`Q`** | Quit | Exits the application |

---

### 📖 Reading the results table

| Column / colour | Meaning |
| :--- | :--- |
| `Green [OK]` | Latency below 80 ms and UDP/53 answering on your ISP |
| `Yellow [OK]` | Latency 80-160 ms, UDP/53 answering |
| `[!]` | Host replies to ICMP but UDP/53 is blocked or hijacked by your ISP |
| `TimeOut` (red) | Server offline or blocked |
| `Cyan` | DoH-only provider — usable over HTTPS/443, not as a plain Windows DNS |
| `DoH:443 = YES [OK]` | Encrypted DNS-over-HTTPS endpoint verified live |
| `DoH:443 = YES [X]` | Provider has DoH, but the endpoint is unreachable from your network |
| `DoH:443 = -` | Provider publishes no public DoH endpoint |

---

### 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

---

<div align="center">
  Made with ❤️ for Iranian developers and gamers
</div>
