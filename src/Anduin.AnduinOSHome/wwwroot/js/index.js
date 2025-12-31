//=====================================
//         The Download page
//=====================================
const allLanguages = [
    {
        code: "en_US",
        name: "English (United States)",
        checksumLabel: "Checksum",
        torrentLabel: "Torrent",
        directLabel: "Direct (HTTP)",
    },
    {
        code: "en_GB",
        name: "English (United Kingdom)",
        checksumLabel: "Checksum",
        torrentLabel: "Torrent",
        directLabel: "Direct (HTTP)",
    },
    {
        code: "zh_CN",
        name: "中文 (中国大陆)",
        checksumLabel: "校验和",
        torrentLabel: "种子",
        directLabel: "源站下载 (HTTP)",
    },
    {
        code: "zh_TW",
        name: "中文 (台灣)",
        checksumLabel: "校驗和",
        torrentLabel: "種子",
        directLabel: "源站下載 (HTTP)",
    },
    {
        code: "zh_HK",
        name: "中文 (香港)",
        checksumLabel: "校驗和",
        torrentLabel: "種子",
        directLabel: "源站下載 (HTTP)",
    },
    {
        code: "ja_JP",
        name: "日本語",
        checksumLabel: "チェックサム",
        torrentLabel: "トレント",
        directLabel: "直接ダウンロード (HTTP)",
    },
    {
        code: "ko_KR",
        name: "한국어",
        checksumLabel: "체크섬",
        torrentLabel: "토렌트",
        directLabel: "직접 다운로드 (HTTP)",
    },
    {
        code: "vi_VN",
        name: "Tiếng Việt",
        checksumLabel: "Mã kiểm tra",
        torrentLabel: "Torrent",
        directLabel: "Tải trực tiếp (HTTP)",
    },
    {
        code: "th_TH",
        name: "ภาษาไทย",
        checksumLabel: "เช็กซัม",
        torrentLabel: "ทอร์เรนต์",
        directLabel: "ดาวน์โหลดโดยตรง (HTTP)",
    },
    {
        code: "de_DE",
        name: "Deutsch",
        checksumLabel: "Prüfsumme",
        torrentLabel: "Torrent",
        directLabel: "Direktdownload (HTTP)",
    },
    {
        code: "fr_FR",
        name: "Français",
        checksumLabel: "Somme de contrôle",
        torrentLabel: "Torrent",
        directLabel: "Téléchargement direct (HTTP)",
    },
    {
        code: "es_ES",
        name: "Español",
        checksumLabel: "Suma de comprobación",
        torrentLabel: "Torrent",
        directLabel: "Descarga directa (HTTP)",
    },
    {
        code: "ru_RU",
        name: "Русский",
        checksumLabel: "Контрольная сумма",
        torrentLabel: "Торрент",
        directLabel: "Прямая загрузка (HTTP)",
    },
    {
        code: "it_IT",
        name: "Italiano",
        checksumLabel: "Somma di controllo",
        torrentLabel: "Torrent",
        directLabel: "Download diretto (HTTP)",
    },
    {
        code: "pt_PT",
        name: "Português",
        checksumLabel: "Soma de verificação",
        torrentLabel: "Torrent",
        directLabel: "Download direto (HTTP)",
    },
    {
        code: "pt_BR",
        name: "Português (Brasil)",
        checksumLabel: "Soma de verificação",
        torrentLabel: "Torrent",
        directLabel: "Download direto (HTTP)",
    },
    {
        code: "ar_SA",
        name: "العربية",
        checksumLabel: "التحقق من الصحة",
        torrentLabel: "التورنت",
        directLabel: "تحميل مباشر (HTTP)",
    },
    {
        code: "nl_NL",
        name: "Nederlands",
        checksumLabel: "Controlegetal",
        torrentLabel: "Torrent",
        directLabel: "Directe download (HTTP)",
    },
    {
        code: "sv_SE",
        name: "Svenska",
        checksumLabel: "Kontrollsumma",
        torrentLabel: "Torrent",
        directLabel: "Direktnedladdning (HTTP)",
    },
    {
        code: "pl_PL",
        name: "Polski",
        checksumLabel: "Suma kontrolna",
        torrentLabel: "Torrent",
        directLabel: "Bezpośrednie pobieranie (HTTP)",
    },
    {
        code: "tr_TR",
        name: "Türkçe",
        checksumLabel: "Kontrol toplamı",
        torrentLabel: "Torrent",
        directLabel: "Doğrudan İndirme (HTTP)",
    },
    {
        code: "ro_RO",
        name: "Română",
        checksumLabel: "Sumă de control",
        torrentLabel: "Torrent",
        directLabel: "Descărcare directă (HTTP)",
    }
];
const defaultLanguageCodes = [
    "en_US", "en_GB", "zh_CN", "zh_TW", "zh_HK", "ja_JP", "ko_KR", "vi_VN",
    "th_TH", "de_DE", "fr_FR", "es_ES", "ru_RU", "it_IT", "pt_PT", "pt_BR",
    "ar_SA", "nl_NL", "sv_SE", "pl_PL", "tr_TR"
];

const languageSelect= document.getElementById('language-select');
const downloadModalEl          = document.getElementById('download-modal');
const downloadModalDescription = document.getElementById('download-modal-description');
const downloadLinksContainer   = document.getElementById('download-links-container');
const downloadModalDescriptionText = downloadModalDescription.textContent;
downloadModalEl.addEventListener('show.bs.modal', event => {
    const btn = event.relatedTarget;
    const version       = btn.dataset.version;
    const latest        = btn.dataset.latest;
    const size          = btn.dataset.size;
    const preferred = btn.dataset.preferedLanguage || '';
    const supportTorrent = btn.dataset.supportTorrent === 'true';
    const supportHttps   = btn.dataset.supportHttps   === 'true';

    const additionalLangsRaw = btn.dataset.additionalLangs || '';
    const additionalLangs = additionalLangsRaw.split(',').filter(Boolean); // e.g., ['ro_RO'] or []

    downloadModalDescription.textContent = `${downloadModalDescriptionText} (${size})`;

    const allowedLanguageCodes = new Set([...defaultLanguageCodes, ...additionalLangs]);

    const languagesToShow = allLanguages.filter(lang => allowedLanguageCodes.has(lang.code));

    languageSelect.innerHTML = '';
    languagesToShow.forEach(lang => {
        const opt = document.createElement('option');
        opt.value       = lang.code;
        opt.textContent = lang.name;
        languageSelect.appendChild(opt);
    });

    const prefLower = preferred.toLowerCase();
    const matches = languagesToShow
        .map(lang => lang.code)
        .filter(code => code.toLowerCase().startsWith(prefLower))
        .sort((a, b) => b.length - a.length);

    languageSelect.value = matches.length
        ? matches[0]
        : languagesToShow.length > 0 ? languagesToShow[0].code : 'en_US';

    languageSelect.onchange = () => {
        renderDownloadLinks({ version, latest, supportTorrent, supportHttps }, languageSelect.value);
    };

    const initialLang = languageSelect.value || (languagesToShow.length > 0 ? languagesToShow[0].code : 'en_US');
    renderDownloadLinks({ version, latest, supportTorrent, supportHttps }, initialLang);
});

function renderDownloadLinks({ version, latest, supportTorrent, supportHttps }, langCode) {
    const lang = allLanguages.find(l => l.code === langCode);
    if (!lang) {
        console.error(`Language code ${langCode} not found in allLanguages array.`);
        return;
    }

    downloadLinksContainer.innerHTML = '';

    const base = `https://download.anduinos.com/${version}/${latest}/AnduinOS-${latest}-${lang.code}`;
    if (supportTorrent) {
        appendLink(`${base}.torrent`, lang.torrentLabel, 'btn-primary');
    }
    if (supportHttps) {
        appendLink(`${base}.iso`, lang.directLabel, 'btn-outline-primary');
    }
    appendLink(`${base}.sha256`,    lang.checksumLabel, 'btn-outline-primary');
}

function appendLink(href, label, btnClass) {
    const a = document.createElement('a');
    a.href      = href;
    a.target    = '_blank';
    a.className = `btn btn-lg btn-pill ${btnClass}`;
    a.textContent = label;
    a.addEventListener("click", function(e){setTimeout(() => {top.location.href = "thankyou.html";}, 500);});
    downloadLinksContainer.appendChild(a);
}

//=====================================
//         The Image Gallery
//=====================================
document.addEventListener('DOMContentLoaded', (() => {
    document.querySelectorAll('img[gallery]').forEach(x => {
        x.addEventListener('click', function () {
            igl_show(this)
        });
    });
}));

function igl_show(img) {
    var iglmodal = document.getElementById('iglmodal');
    var iglmodalImg = document.getElementById('iglmodal-img');
    iglmodal.style.display = 'flex';
    iglmodalImg.src = img.src.replace('_comp', '').replace('.webp', '.png');
    iglmodalImg.onclick = function (event) {
        event.stopPropagation();
    };
}

function igl_hide() {
    document.getElementById('iglmodal').style.display = 'none';
}

