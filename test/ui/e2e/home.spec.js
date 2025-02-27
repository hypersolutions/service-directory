// @ts-check
import { test, expect } from '@playwright/test';
import { description, enterPostcode, clickSearch, getListItem } from './page-objects/home-page-objects';

test('home has description', async ({ page }) => {
  await page.goto('http://localhost:3000/');

  const locator = description(page);
  await expect(locator).toHaveText(
      'Use the search tools below to find services within a certain distance of the entered postcode. ' +
      'These will be ordered from the closest to furthest.');
});

test('search for services by postcode', async ({ page }) => {
  await page.goto('http://localhost:3000/');
  await enterPostcode(page, 'BS3 2EJ');
  
  await clickSearch(page);
  
  expect(getListItem(page, 'ANDYSMANCLUB')).toBeTruthy();
  expect(getListItem(page, 'Autism Advice - Bristol')).toBeTruthy();
  expect(getListItem(page, 'Activity Group - Redfield (St Anne\'s)')).toBeTruthy();
});

