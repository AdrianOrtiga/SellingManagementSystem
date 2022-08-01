

const DEFAULT_ERROR_MESSAGE = "The page is broken. OMG!"

const addConceptBtn = document.getElementById("addConceptBtn")
const conceptsWrapper = document.getElementById("conceptsWrapper")

// if edit view should fill the concepts fields
window.addEventListener('load', getConceptsForEditView)

// add new concept
addConceptBtn.addEventListener('click', addNewConcept)


// Calculate concept fields 
function fillProductId(productNameInput) {
    const productIdInput = productNameInput.previousElementSibling

    if (productIdInput.classList.contains('product-concept')) {
        const id = parseInt(document.querySelector(`option[value="${productNameInput.value}"]`).textContent)
       
        productIdInput.setAttribute("value", id)

        // get product price per unit 
        fillConceptFields()

        return
    }

    throw new Error(DEFAULT_ERROR_MESSAGE)
}

function getFormData() {
    const formElement = document.querySelector('form')
    const data = new URLSearchParams(new FormData(formElement));

    return data
}

function getConceptsForEditView() {
    fetchRequest({ action: 'GetConceptsForEditView', method: 'POST', body: getFormData() })
}

function addNewConcept() {
    fetchRequest({ action: 'AddNewConcept', method: 'POST', body: getFormData() })
}

function deleteConcept(target) {
    target.closest('.card').remove()

    fetchRequest({ action: `DeleteConcept`, method: 'POST', body: getFormData() })
}


function fillConceptFields() {
    fetchRequest({ action: 'FillConceptFields', method: 'POST', body: getFormData() })
}

//calculate the amount
function calculateAmount({ target }) {
    const card = target.closest('.card')

    const amountInput = card.querySelector('.amount-concept')
    const pricePerUnit = card.querySelector('.priceperunit-concept').value
    const quantity = card.querySelector('.quantity-concept').value

    const amount = (pricePerUnit * quantity).toFixed(2)

    amountInput.setAttribute("value", amount)

    calculateTotalAmount()
}

function calculateTotalAmount(){
    const amountInputs = document.querySelectorAll('.amount-concept')

    let totalAmount = 0.00
    amountInputs.forEach(amountInput => {
        totalAmount = totalAmount + parseFloat(amountInput.value)
    })

    const totalInput = document.getElementById('total-sell-amount')

    totalInput.setAttribute("value", totalAmount.toFixed(2))
}

function fetchRequest({ action, method = 'GET', body }) {
    const pathHttpReq = `${window.location.origin}/Sell/${action}`


    fetch(pathHttpReq, { method, body })
        .then(response => {
            return response.text()
        })
        .then(data => {
            // transfor text from the response into html
            const parser = new DOMParser()
            const newConceptForm = parser.parseFromString(data, 'text/html')

            conceptsWrapper.firstChild.remove()
            conceptsWrapper.appendChild(newConceptForm.body.firstChild)

            // if product change go an get the product data from database
            const productInputs = document.querySelectorAll('.product-concept')

            productInputs.forEach(element => {
                element.onchange = fillConceptFields
            })

            // if quantity change we should update the amount
            updateAllConceptsAmount()
        })
        .catch(err => {
            console.log(err)
        })
}

function updateAllConceptsAmount() {
    const quantityInputs = document.querySelectorAll('.quantity-concept')

    quantityInputs.forEach(element => {
        element.onchange = calculateAmount

        calculateAmount({ target: element })
    })
}