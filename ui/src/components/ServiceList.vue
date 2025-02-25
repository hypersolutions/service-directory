<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <ItemFilter :loading="loading" @filterClicked="onFilterClick" @resetFilterClicked="onResetFilter"/>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" v-if="loading">
        <ProgressBar message="Loading services"/>
      </v-col>
      <v-col cols="12" v-else>
        <v-list lines="one" class="overflow-y-auto" max-height="800">
          <v-list-item v-for="item in filteredServices" :key="item.id">
            <ItemOverview :item="item" @itemClicked="onItemClicked"/>
          </v-list-item>
        </v-list>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
const services = defineModel('services', {default: []});
const filteredServices = defineModel('filteredServices', {default: []});
const loading = defineModel('loading', {default: false});

async function getServices() {
  try {
    loading.value = true;
    services.value = [];

    const url = '/api/service';
    const options = {method: 'GET'};
    const response = await fetch(url, options);

    if (response.ok) {
      services.value = await response.json();
    }
  }
  catch (error) {
    console.error(error);
  }
  finally {
    filteredServices.value = services.value;
    // Allow the progress to run for a second or two
    setTimeout(() => (loading.value = false), 2000);
  }
}

function onFilterClick(args) {
  if (args.filter !== '') {
    const regex = new RegExp(args.filter, 'i');
    filteredServices.value = services.value.filter(svc => regex.test(svc.name) || regex.test(svc.description));
  }
  else {
    onResetFilter();
  }
}

function onItemClicked(args) {
  console.log(`Item ${args.id} clicked`);
}

function onResetFilter() {
  filteredServices.value = services.value;
}

getServices();

</script>

<style scoped>

</style>
