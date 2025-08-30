export async function resizeImage(file, maxWidth, maxHeight, quality = 0.8) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = function (event) {
            const img = new Image();
            img.onload = function () {
                const canvas = document.createElement('canvas');
                let width = img.width;
                let height = img.height;

                if (width > maxWidth) {
                    height *= maxWidth / width;
                    width = maxWidth;
                }
                if (height > maxHeight) {
                    width *= maxHeight / height;
                    height = maxHeight;
                }

                canvas.width = width;
                canvas.height = height;
                const ctx = canvas.getContext('2d');
                ctx.drawImage(img, 0, 0, width, height);

                canvas.toBlob(blob => {
                    const reader = new FileReader();
                    reader.onloadend = () => {
                        resolve(new Uint8Array(reader.result));
                    };
                    reader.readAsArrayBuffer(blob);
                }, 'image/jpeg', quality);
            };
            img.src = event.target.result;
        };
        reader.onerror = reject;
        reader.readAsDataURL(file);
    });
}