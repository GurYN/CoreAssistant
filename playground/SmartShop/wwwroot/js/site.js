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

connection.on("DescriptionResult", (content) => {
    const send = document.getElementById("send");
    send.disabled = false;
    send.innerHTML = "Generate";

    const addKeyword = document.getElementById("add-keyword");
    addKeyword.disabled = false;

    const clear = document.getElementById("clear");
    clear.disabled = false;

    const description = document.getElementById("description");
    description.innerHTML = content;
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

    keywordBadges.innerHTML = "";
    prompt.value = "";
    description.innerHTML = "Product Description...";
});

const send = document.getElementById("send");
send.addEventListener("click", async () => {
    const description = document.getElementById("description");
    description.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Generating Product Description...';

    const addKeyword = document.getElementById("add-keyword");
    addKeyword.disabled = true;

    const clear = document.getElementById("clear");
    clear.disabled = true;

    send.disabled = true;
    send.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Generating...';

    const prompt = document.getElementById("prompt").value;
    await connection.invoke("GenerateDescription", prompt);
});