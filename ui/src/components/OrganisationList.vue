<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <ItemFilter :loading="loading" @filterClicked="onFilterClick" @resetFilterClicked="onResetFilter"/>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" v-if="loading">
        <ProgressBar message="Loading organisations"/>
      </v-col>
      <v-col cols="12" v-else>
        <v-list lines="one" class="overflow-y-auto" max-height="800">
          <v-list-item v-for="item in filteredOrganisations" :key="item.id">
            <ItemOverview :item="item" @itemClicked="onItemClicked"/>
          </v-list-item>
        </v-list>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
const organisations = defineModel('organisations', {default: []});
const filteredOrganisations = defineModel('filteredOrganisations', {default: []});
const loading = defineModel('loading', {default: false});

async function getOrganisations() {
  try {
    loading.value = true;
    organisations.value = [];

    const url = '/api/organisation';
    const options = {method: 'GET'};
    const response = await fetch(url, options);

    if (response.ok) {
      organisations.value = await response.json();
    }
  }
  catch (error) {
    console.error(error);
  }
  finally {
    filteredOrganisations.value = organisations.value;
    // Allow the progress to run for a second or two
    setTimeout(() => (loading.value = false), 2000);
  }
}

function onItemClicked(args) {
  console.log(`Item ${args.id} clicked`);
}

function onFilterClick(args) {
  if (args.filter !== '') {
    const regex = new RegExp(args.filter, 'i');
    filteredOrganisations.value = organisations.value.filter(org => regex.test(org.name) || regex.test(org.description));
  }
  else {
    onResetFilter();
  }
}

function onResetFilter() {
  filteredOrganisations.value = organisations.value;
}

getOrganisations();

</script>

<style scoped>

</style>
