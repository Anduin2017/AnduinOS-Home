const languages = [
  {
    code: "en_US",
    name: "English (United States)",
    checksumLabel: "Checksum",
  },
  { code: "zh_CN", name: "中文 (中国大陆)", checksumLabel: "校验和" },
  { code: "zh_TW", name: "中文 (台灣)", checksumLabel: "校驗和" },
  { code: "zh_HK", name: "中文 (香港)", checksumLabel: "校驗和" },
  { code: "ja_JP", name: "日本語", checksumLabel: "チェックサム" },
  { code: "ko_KR", name: "한국어", checksumLabel: "체크섬" },
  { code: "vi_VN", name: "Tiếng Việt", checksumLabel: "Kiểm tra" },
  { code: "th_TH", name: "ภาษาไทย", checksumLabel: "ตรวจสอบ" },
  { code: "de_DE", name: "Deutsch", checksumLabel: "Prüfsumme" },
  { code: "fr_FR", name: "Français", checksumLabel: "Somme de contrôle" },
  { code: "es_ES", name: "Español", checksumLabel: "Suma de comprobación" },
  { code: "ru_RU", name: "Русский", checksumLabel: "Контрольная сумма" },
  { code: "it_IT", name: "Italiano", checksumLabel: "Somma di controllo" },
  { code: "pt_PT", name: "Português", checksumLabel: "Soma de verificação" },
  { code: "pt_BR", name: "Português (Brasil)", checksumLabel: "Soma de verificação"},
  { code: "ar_SA", name: "العربية", checksumLabel: "التحقق من الصحة" },
  { code: "nl_NL", name: "Nederlands", checksumLabel: "Controlegetal" },
  { code: "sv_SE", name: "Svenska", checksumLabel: "Kontrollsumma" },
  { code: "pl_PL", name: "Polski", checksumLabel: "Suma kontrolna" },
  { code: "tr_TR", name: "Türkçe", checksumLabel: "Kontrol toplamı" },
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
    const response = await fetch("/versions.json");
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
  col.className = "col-sm-4 mb-3 mb-md-0";

  const card = document.createElement("div");
  card.className = "card text-center h-100";

  if (versionObj.recommended) {
    card.classList.add("middle-card");
  }

  const cardBody = document.createElement("div");
  cardBody.className = "card-body d-flex flex-column";

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
    versionObj.latestVersion,
  ];
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

// Function to open and populate the download modal
function openDownloadModal(versionObj) {
  downloadModalTitle.textContent = `Download AnduinOS ${versionObj.version}`;
  downloadModalDescription.textContent =
    "To download AnduinOS, select the correct edition below: (1.8GB)";
  downloadLinksContainer.innerHTML = ""; // Clear previous links

  languages.forEach((lang) => {
    const isoLink = document.createElement("a");
    isoLink.href = `https://download.anduinos.com/${versionObj.version}/${versionObj.latest}/AnduinOS-${versionObj.latest}-${lang.code}.iso`;
    isoLink.target = "_blank";
    isoLink.className = "btn btn-primary btn-lg btn-pill me-2 mb-2";
    isoLink.textContent = `ISO - ${lang.name}`;

    const checksumLink = document.createElement("a");
    checksumLink.href = `https://download.anduinos.com/${versionObj.version}/${versionObj.latest}/AnduinOS-${versionObj.latest}-${lang.code}.sha256`;
    checksumLink.target = "_blank";
    checksumLink.className = "btn btn-outline-primary btn-lg btn-pill mb-2";
    checksumLink.textContent = lang.checksumLabel;

    // Wrap links in a div for better layout
    const linkDiv = document.createElement("div");
    linkDiv.className = "mb-2";
    linkDiv.appendChild(isoLink);
    linkDiv.appendChild(checksumLink);

    downloadLinksContainer.appendChild(linkDiv);
  });

  downloadModal.show();
}

// Function to generate version cards dynamically
function generateVersionCards(versions) {
  versions.forEach((version) => {
    const card = createVersionCard(version);
    versionCardsContainer.appendChild(card);
  });
}

// Fetch and populate versions on page load
fetchVersions();
