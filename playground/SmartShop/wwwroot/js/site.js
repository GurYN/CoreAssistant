const connection = new signalR.HubConnectionBuilder()
    .withUrl("/assistant")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.log("Connected to hub.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

connection.on("ImageResult", (url) => {
    const img = document.getElementById("photo");
    img.src = url;
});

connection.on("TitleResult", (text) => {
    const title = document.getElementById("title");
    title.innerHTML = text;
});

connection.on("DescriptionResult", (content) => {
    const description = document.getElementById("description");
    description.innerHTML = content;
});

connection.on("Done", () => {
    const send = document.getElementById("send");
    send.disabled = false;
    send.innerHTML = "Generate";

    const addKeyword = document.getElementById("add-keyword");
    addKeyword.disabled = false;

    const clear = document.getElementById("clear");
    clear.disabled = false;
});

start();

const addKeyword = document.getElementById("add-keyword");
addKeyword.addEventListener("click", () => {
    const keyword = document.getElementById("keyword");
    const keywordBadges = document.getElementById("keyword-badges");
    const prompt = document.getElementById("prompt");

    keywordBadges.innerHTML += ` <span class="badge bg-success">${keyword.value}</span>`;
    prompt.value += prompt.value.length == 0 ? keyword.value : `, ${keyword.value}`;
    keyword.value = "";
});

const clear = document.getElementById("clear");
clear.addEventListener("click", () => {
    const keywordBadges = document.getElementById("keyword-badges");
    const prompt = document.getElementById("prompt");
    const description = document.getElementById("description");
    const title = document.getElementById("title");
    const img = document.getElementById("photo");

    keywordBadges.innerHTML = "";
    prompt.value = "";
    description.innerHTML = "Product Description...";
    title.innerHTML = "Product Title...";
    img.src = "https://placehold.co/600x400?text=Product Photo...";
});

const send = document.getElementById("send");
send.addEventListener("click", async () => {
    const addKeyword = document.getElementById("add-keyword");
    addKeyword.disabled = true;

    const clear = document.getElementById("clear");
    clear.disabled = true;

    send.disabled = true;
    send.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Generating...';

    const prompt = document.getElementById("prompt").value;
    await connection.invoke("GenerateDescription", prompt);
});