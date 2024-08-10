"use strict";

function loadShortcuts(shortcuts) {
    q("#shortcuts").value = JSON.stringify(shortcuts, null, 2);
}

function parseShortcuts() {
    const json = q("#shortcuts").value;
    const shortcuts = JSON.parse(json);
    return shortcuts;
}

function saveChanges() {
    try {
        const shortcuts = parseShortcuts();
        chrome.storage.local.set({ shortcuts });
        chrome.storage.local.get()
            .then(storage => loadShortcuts(storage.shortcuts));
        q("#error").innerText = null;
    } catch (error) {
        q("#error").innerText = error.message;
    }
}

function switchTab(event) {
    q(".tab-header").forEach(x => x.classList.remove("active"))
    q(".tab-content").forEach(x => x.classList.add("hidden"))
    event.currentTarget.classList.add("active");
    q(event.currentTarget.dataset.contentSelector).classList.remove("hidden");
}

function init(storage) {
    loadShortcuts(storage.shortcuts);
    q("#save-button").addEventListener("click", saveChanges);
    q(".tab-header").forEach(x => x.addEventListener("click", switchTab));
}

migrate()
    .then(init);
