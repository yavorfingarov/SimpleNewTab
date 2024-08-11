"use strict";

const metadataUrl = "https://simplenewtab.azurewebsites.net/api/image-metadata/latest";

const iconSize = 24;

async function fetchImageMetadata() {
    try {
        const response = await fetch(metadataUrl);
        if (response.ok) {
            return await response.json();
        }
    } catch  { }
}

function clone(id) {
    const element = q(id).content.children[0];
    const clone = element.cloneNode(true);
    return clone;
}

function setBackground(imageMetadata) {
    if (imageMetadata) {
        document.body.style.backgroundImage = `url(${imageMetadata.url})`;
        const title = q("#title");
        title.href = imageMetadata.quizUrl;
        title.innerText = imageMetadata.title;
        const copyright = q("#copyright");
        copyright.href = imageMetadata.copyrightUrl;
        copyright.innerText = imageMetadata.copyright;
    } else {       
        q("#card-toggle").classList.add("hidden");
    }
    return imageMetadata;
}

function cacheImageMetadata(imageMetadata) {
    if (imageMetadata) {
        chrome.storage.local.set({ imageMetadata });
    }
    return imageMetadata;
}

function cacheImage(imageMetadata) {
    if (imageMetadata) {
        fetch(imageMetadata.url);
    }
    return imageMetadata;
}

function createShortcutElement(shortcut) {
    const shortcutElement = clone("#shortcut");
    shortcutElement.href = shortcut.url;
    const img = q("img", shortcutElement);
    const faviconUrl = new URL(`chrome-extension://${chrome.runtime.id}/_favicon/`);
    faviconUrl.searchParams.append("pageUrl", shortcut.url);
    faviconUrl.searchParams.append("size", iconSize);
    img.src = faviconUrl.href;
    img.width = iconSize;
    img.height = iconSize;
    q(".label", shortcutElement).innerText = shortcut.name;
    return shortcutElement;
}

function addShortcuts(shortcuts) {
    if (!shortcuts) {
        return;
    }
    const shortcutsElement = q("#shortcuts");
    shortcutsElement.replaceChildren();
    for (const shortcut of shortcuts) {
        const shortcutElement = createShortcutElement(shortcut);
        shortcutsElement.append(shortcutElement);
    }
}

function toggleCard() {
    q("#card-toggle-up").classList.toggle("hidden");
    q("#card-toggle-down").classList.toggle("hidden");
    q("#card").classList.toggle("hidden");
    q("#shortcuts").classList.toggle("hidden");
    q("#shadow").classList.toggle("hidden");
}

function init(storage) {
    if (storage.imageMetadata) {
        setBackground(storage.imageMetadata);
        const expiration = Date.parse(storage.imageMetadata.expiration);
        if (expiration < Date.now()) {
            fetchImageMetadata()
                .then(cacheImageMetadata)
                .then(cacheImage);
        }
    } else  {
        fetchImageMetadata()
            .then(setBackground)
            .then(cacheImageMetadata);
    }
    addShortcuts(storage.shortcuts);
    chrome.storage.local.onChanged
        .addListener(storage => addShortcuts(storage.shortcuts?.newValue));
    q("#card-toggle").addEventListener("click", toggleCard);
}

migrate()
    .then(init);
