const url_server = `https://localhost:7044`

if(!apiToken) window.location.href = "../html/login.html";
else {
    async function loadProducts() {
        const url_products = `${url_server}/api/apiproducts`
        return await fetch(
            url_products,
            {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${apiToken}`
                }
            }
        ).then(response => {
            if(!response.ok)
                throw new Error('Fail to get products ...')
            return response.json()
        }).then(products => {
            let result = ''
            products.forEach(p => {
                result += 
                `
                    <div class="card" style="width: 18rem;">
                        <img src="./img/products.jpg" class="card-img-top" alt="">
                        <div class="card-body">
                            <h5 id="titleId" class="card-title">${p.name}</h5>
                            <p id="descriptionId" class="card-text">${p.description}</p>
                            <p id="priceId" class="card-text">${p.price}</p>
                            <a href="#" class="btn btn-primary">buy</a>
                        </div>
                    </div>
                `
            });
            document.getElementById("parent_products")
            .innerHTML = result
        })
    }
    loadProducts()
}