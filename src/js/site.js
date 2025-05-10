const languages = [
  { code: "en_US", name: "English (United States)", checksumLabel: "Checksum", torrentLabel: "Torrent", directLabel: "Direct (HTTP)" },
  { code: "en_GB", name: "English (United Kingdom)", checksumLabel: "Checksum", torrentLabel: "Torrent", directLabel: "Direct (HTTP)" },
  { code: "zh_CN", name: "中文 (中国大陆)", checksumLabel: "校验和", torrentLabel: "种子", directLabel: "源站下载 (HTTP)" },
  { code: "zh_TW", name: "中文 (台灣)", checksumLabel: "校驗和", torrentLabel: "種子", directLabel: "源站下載 (HTTP)" },
  { code: "zh_HK", name: "中文 (香港)", checksumLabel: "校驗和", torrentLabel: "種子", directLabel: "源站下載 (HTTP)" },
  { code: "ja_JP", name: "日本語", checksumLabel: "チェックサム", torrentLabel: "トレント", directLabel: "直接ダウンロード (HTTP)" },
  { code: "ko_KR", name: "한국어", checksumLabel: "체크섬", torrentLabel: "토렌트", directLabel: "직접 다운로드 (HTTP)" },
  { code: "vi_VN", name: "Tiếng Việt", checksumLabel: "Mã kiểm tra", torrentLabel: "Torrent", directLabel: "Tải trực tiếp (HTTP)" },
  { code: "th_TH", name: "ภาษาไทย", checksumLabel: "เช็กซัม", torrentLabel: "ทอร์เรนต์", directLabel: "ดาวน์โหลดโดยตรง (HTTP)" },
  { code: "de_DE", name: "Deutsch", checksumLabel: "Prüfsumme", torrentLabel: "Torrent", directLabel: "Direktdownload (HTTP)" },
  { code: "fr_FR", name: "Français", checksumLabel: "Somme de contrôle", torrentLabel: "Torrent", directLabel: "Téléchargement direct (HTTP)" },
  { code: "es_ES", name: "Español", checksumLabel: "Suma de comprobación", torrentLabel: "Torrent", directLabel: "Descarga directa (HTTP)" },
  { code: "ru_RU", name: "Русский", checksumLabel: "Контрольная сумма", torrentLabel: "Торрент", directLabel: "Прямая загрузка (HTTP)" },
  { code: "it_IT", name: "Italiano", checksumLabel: "Somma di controllo", torrentLabel: "Torrent", directLabel: "Download diretto (HTTP)" },
  { code: "pt_PT", name: "Português", checksumLabel: "Soma de verificação", torrentLabel: "Torrent", directLabel: "Download direto (HTTP)" },
  { code: "pt_BR", name: "Português (Brasil)", checksumLabel: "Soma de verificação", torrentLabel: "Torrent", directLabel: "Download direto (HTTP)" },
  { code: "ar_SA", name: "العربية", checksumLabel: "التحقق من الصحة", torrentLabel: "التورنت", directLabel: "تحميل مباشر (HTTP)" },
  { code: "nl_NL", name: "Nederlands", checksumLabel: "Controlegetal", torrentLabel: "Torrent", directLabel: "Directe download (HTTP)" },
  { code: "sv_SE", name: "Svenska", checksumLabel: "Kontrollsumma", torrentLabel: "Torrent", directLabel: "Direktnedladdning (HTTP)" },
  { code: "pl_PL", name: "Polski", checksumLabel: "Suma kontrolna", torrentLabel: "Torrent", directLabel: "Bezpośrednie pobieranie (HTTP)" },
  { code: "tr_TR", name: "Türkçe", checksumLabel: "Kontrol toplamı", torrentLabel: "Torrent", directLabel: "Doğrudan İndirme (HTTP)" },
];

const versionCardsContainer = document.getElementById("version-cards");
const downloadModal = new bootstrap.Modal(
  document.getElementById("download-modal")
);
const downloadModalTitle = document.getElementById("download-modal-title");
const downloadModalDescription = document.getElementById(
  "download-modal-description"
);
const downloadLinksContainer = document.getElementById(
  "download-links-container"
);

// Function to fetch versions from API
async function fetchVersions() {
  try {
    const response = await fetch("/versions.json?version=20250510");
    if (!response.ok) {
      throw new Error(`Failed to fetch versions: ${response.status}`);
    }
    const versions = await response.json();
    generateVersionCards(versions);
  } catch (error) {
    console.error("Error fetching versions:", error);
  }
}

// Function to create version card
function createVersionCard(versionObj) {
  const col = document.createElement("div");
  col.className = "col-sm-4 mb-5 mb-md-0";

  const card = document.createElement("div");
  card.className = "card text-center h-100";

  if (versionObj.largeCard) {
    card.classList.add("middle-card");
  }

  const cardBody = document.createElement("div");
  cardBody.className = "card-body d-flex flex-column";

  if (versionObj.notRecommended) {
    cardBody.style.setProperty("color", "#bbb", "important");
  }

  const versionInfo = document.createElement("div");
  versionInfo.className = "mb-4";

  const h5 = document.createElement("h5");
  h5.textContent = versionObj.codename;

  const spanVersion = document.createElement("span");
  spanVersion.className = versionObj.recommended ? "display-2" : "display-5";
  spanVersion.textContent = versionObj.version;

  const spanType = document.createElement("span");
  spanType.textContent = versionObj.type;

  versionInfo.appendChild(h5);
  versionInfo.appendChild(spanVersion);
  versionInfo.appendChild(document.createElement("br")); // Line break
  versionInfo.appendChild(spanType);

  const h6Includes = document.createElement("h6");
  h6Includes.className = "mt-3";
  h6Includes.textContent = "Includes:";

  const ul = document.createElement("ul");
  ul.className = "list-unstyled";
  const includes = [
    versionObj.support,
    versionObj.gnome,
    versionObj.packages,
    versionObj.kernel,
    versionObj.releaseDate,
    "Latest version " + versionObj.latest
  ];

  if (versionObj.additionalText) {
    includes.push(versionObj.additionalText);
  }
  includes.forEach((item) => {
    const li = document.createElement("li");
    li.className = "mb-2";
    li.textContent = item;
    ul.appendChild(li);
  });

  if (versionObj.recommended) {
    const li = document.createElement("li");
    li.className = "mb-2";
    const badge = document.createElement("span");
    badge.className = "badge badge-subtle-primary";
    badge.textContent = "Recommended";
    li.appendChild(badge);
    ul.appendChild(li);
  }

  const downloadButton = document.createElement("button");
  downloadButton.type = "button";
  downloadButton.className = `btn btn-lg btn-pill ${
    versionObj.recommended ? "btn-primary" : "btn-outline-primary"
  }`;
  downloadButton.textContent = "Download";
  downloadButton.setAttribute("data-version", versionObj.version);
  downloadButton.addEventListener("click", () => {
    openDownloadModal(versionObj);
  });

  const downloadDiv = document.createElement("div");
  downloadDiv.className = "mt-auto";
  downloadDiv.appendChild(downloadButton);

  cardBody.appendChild(versionInfo);
  cardBody.appendChild(h6Includes);
  cardBody.appendChild(ul);
  cardBody.appendChild(downloadDiv);

  card.appendChild(cardBody);
  col.appendChild(card);

  return col;
}

function renderDownloadLinks(versionObj, langCode) {
  const lang = languages.find(l => l.code === langCode);
  downloadLinksContainer.innerHTML = "";  // 清空

  // direct (HTTP)
  const isoLink = document.createElement("a");
  isoLink.href = `https://download.anduinos.com/${versionObj.version}/${versionObj.latest}/AnduinOS-${versionObj.latest}-${lang.code}.iso`;
  isoLink.target = "_blank";
  isoLink.className = "btn btn-outline-primary btn-lg btn-pill";
  isoLink.textContent = lang.directLabel;
  downloadLinksContainer.appendChild(isoLink);

  // torrent
  if (versionObj.supportTorrent) {
    const torrentLink = document.createElement("a");
    torrentLink.href = `https://download.anduinos.com/${versionObj.version}/${versionObj.latest}/AnduinOS-${versionObj.latest}-${lang.code}.torrent`;
    torrentLink.target = "_blank";
    torrentLink.className = "btn btn-primary btn-lg btn-pill";
    torrentLink.textContent = lang.torrentLabel;
    downloadLinksContainer.appendChild(torrentLink);
  }

  // sha256
  const checksumLink = document.createElement("a");
  checksumLink.href = `https://download.anduinos.com/${versionObj.version}/${versionObj.latest}/AnduinOS-${versionObj.latest}-${lang.code}.sha256`;
  checksumLink.target = "_blank";
  checksumLink.className = "btn btn-outline-primary btn-lg btn-pill";
  checksumLink.textContent = lang.checksumLabel;
  downloadLinksContainer.appendChild(checksumLink);
}

function openDownloadModal(versionObj) {
  downloadModalTitle.textContent = `Download AnduinOS ${versionObj.version}`;
  downloadModalDescription.textContent =
    "To download AnduinOS, select the correct edition below: (1.8GB)";

  const languageSelect = document.getElementById("language-select");
  languageSelect.innerHTML = "";
  languages.forEach(lang => {
    const opt = document.createElement("option");
    opt.value = lang.code;
    opt.textContent = lang.name;
    languageSelect.appendChild(opt);
  });

  languageSelect.onchange = () => {
    renderDownloadLinks(versionObj, languageSelect.value);
  };

  renderDownloadLinks(versionObj, languageSelect.value);

  downloadModal.show();
}

// Function to generate version cards dynamically
function generateVersionCards(versions) {
  versions
  .filter((version) => version.isVisible)
  .forEach((version) => {
    const card = createVersionCard(version);
    versionCardsContainer.appendChild(card);
  });
}

// Fetch and populate versions on page load
fetchVersions();
