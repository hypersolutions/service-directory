export const description = page => page.getByTestId('description');

export const enterPostcode = async (page, postcode) => await page.getByTestId('postcode-field').fill(postcode);

export const clickSearch = async page => await page.getByTestId('search-button').click();

export const getListItem = async (page, text) => await page.getByText(text);